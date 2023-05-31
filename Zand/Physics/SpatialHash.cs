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

        public SpatialHash(int cellSize)
        {
            _conversionFactor = 1d / cellSize;
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

        public IReadOnlyCollection<ICollider> GetWithin(double distance, Vector2 position)
        {
            var colliders = new Collection<ICollider>();
            int layerCount = (int)Math.Ceiling(distance * _conversionFactor);


            return colliders;
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
            int x = (int)Math.Floor(vector.X * _conversionFactor);
            int y = (int)Math.Floor(vector.Y * _conversionFactor);

            return (long)x << 32 | (long)(uint)y;
        }

        public void Reset()
        {
            _grid.Clear();
        }
    }
}
