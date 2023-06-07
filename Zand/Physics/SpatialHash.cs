using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zand.ECS.Components;

namespace Zand.Physics
{
    class SpatialHash
    {
        private Dictionary<long, List<ICollider>> _grid;
        private Dictionary<int, List<long>> _entityCoordinates;
        private double _conversionFactor;
        private int _cellSize;

        public SpatialHash(int cellSize)
        {
            _conversionFactor = 1d / cellSize;
            _cellSize = cellSize;
            _grid = new Dictionary<long, List<ICollider>>(1000);
            _entityCoordinates = new Dictionary<int, List<long>>();
        }

        public IReadOnlyCollection<ICollider> GetNearby(Vector2 position)
        {
            long cellHash = GetCellHash(position);
            if (!CellExists(cellHash))
            {
                return new Collection<ICollider>();
            }
            return _grid[cellHash];
        }

        public IReadOnlyCollection<ICollider> GetWithin(double distance, Vector2 originalPosition)
        {
            var colliders = new List<ICollider>();
            int layerCount = CalculateLayerCount(distance, originalPosition);
            var positionsToSearch = new Queue<Vector2>();
            var searchedPositions = new HashSet<Vector2>();
            positionsToSearch.Enqueue(originalPosition);

            while (positionsToSearch.Count > 0)
            {
                Vector2 searchPosition = positionsToSearch.Dequeue();
                colliders.AddRange(GetNearby(searchPosition));
                searchedPositions.Add(searchPosition);

                // Determine adjacent positions
                var up = searchPosition + new Vector2(searchPosition.X, searchPosition.Y - _cellSize);
                var down = searchPosition + new Vector2(searchPosition.X, searchPosition.Y + _cellSize);
                var left = searchPosition + new Vector2(searchPosition.X - _cellSize, searchPosition.Y);
                var right = searchPosition + new Vector2(searchPosition.X + _cellSize, searchPosition.Y);
                Vector2[] canidatePositions = new Vector2[]
                {
                    up, down, left, right
                };

                // Determine adjacent position eligibility
                foreach (var canidatePosition in canidatePositions)
                {
                    if (!searchedPositions.Contains(canidatePosition))
                    {
                        
                        // TODO ONLY if grid touches original distance somehow

                        positionsToSearch.Enqueue(canidatePosition);
                    }
                }

            }


            return colliders;
        }

        private int CalculateLayerCount(double distance, Vector2 position)
        {
            return (int)Math.Ceiling(distance * _conversionFactor);
        }

        public void AddCollider(ICollider collider)
        {
            AddToCell(GetCellHash(collider.BottomRight), collider);
            AddToCell(GetCellHash(collider.TopLeft), collider);
            AddToCell(GetCellHash(collider.TopRight), collider);
            AddToCell(GetCellHash(collider.BottomLeft), collider);
        }

        private void AddToCell(long cellHash, ICollider collider)
        {
            // Create New Cell if none exists
            if (!CellExists(cellHash))
            {
                _grid.Add(cellHash, new List<ICollider>(8));
            }

            // Add collider if it is not already there
            if (!ColliderExists(cellHash, collider))
            {
                _grid[cellHash].Add(collider);
                SaveEntityCellCoords(collider.Entity.Id, cellHash);
            }
        }

        private void SaveEntityCellCoords(int id, long cellHash)
        {
            if (!_entityCoordinates.ContainsKey(id))
            {
                _entityCoordinates.Add(id, new List<long>());
            }

            _entityCoordinates[id].Add(cellHash);
        }

        public void RemoveCollider(ICollider collider)
        {
            // No need to remove if it's not there
            if (!_entityCoordinates.ContainsKey(collider.Entity.Id))
            {
                return;
            }

            // Remove from previous cells
            foreach (var cellCoord in _entityCoordinates[collider.Entity.Id])
            {
                _grid[cellCoord].Remove(collider);
            }

            // Remove this entities previous cell coords
            _entityCoordinates[collider.Entity.Id].Clear();
        }

        private bool ColliderExists(long cellHash, ICollider circleCollider)
        {
            foreach (var collider in _grid[cellHash])
            {
                if (collider == circleCollider)
                {
                    return true;
                }
            }

            return false;
        }
        private bool CellExists(long cellHash)
        {
            return _grid.ContainsKey(cellHash);
        }

        private long GetCellHash(Vector2 vector)
        {
            int x = GetCellAxis(vector.X);
            int y = GetCellAxis(vector.Y);

            return (long)x << 32 | (long)(uint)y;
        }

        private int GetCellAxis(float axixValue)
        {
            return (int)Math.Floor(axixValue * _conversionFactor);
        }

        public void Reset()
        {
            _grid.Clear();
        }
    }
}
