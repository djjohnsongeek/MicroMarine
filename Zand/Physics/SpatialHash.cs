using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Zand.ECS.Components;

namespace Zand.Physics
{
    class SpatialHash
    {
        private Dictionary<long, List<CircleCollider>> _grid;
        private Dictionary<uint, List<long>> _entityCoordinates;
        private double _conversionFactor;

        public SpatialHash(int cellSize)
        {
            _conversionFactor = 1d / cellSize;
            _grid = new Dictionary<long, List<CircleCollider>>(1000);
            _entityCoordinates = new Dictionary<uint, List<long>>();
        }

        public IReadOnlyCollection<CircleCollider> GetNearby(Vector2 position)
        {
            long cellHash = GetCellHash(position);
            if (!CellExists(cellHash))
            {
                return new Collection<CircleCollider>();
            }
            return _grid[cellHash];
        }

        public void AddCollider(CircleCollider collider)
        {
            // store pos to reduce excess math
            Vector2 pos = collider.Center;

            Vector2 bottomRight = new Vector2(pos.X + collider.Radius, pos.Y + collider.Radius);
            Vector2 topLeft = new Vector2(pos.X - collider.Radius, pos.Y - collider.Radius);
            Vector2 topRight = new Vector2(bottomRight.X, topLeft.Y);
            Vector2 bottomLeft = new Vector2(topLeft.X, bottomRight.Y);

            AddToCell(GetCellHash(bottomRight), collider);
            AddToCell(GetCellHash(topLeft), collider);
            AddToCell(GetCellHash(topRight), collider);
            AddToCell(GetCellHash(bottomLeft), collider);
        }

        private void AddToCell(long cellHash, CircleCollider collider)
        {
            // Create New Cell if none exists
            if (!CellExists(cellHash))
            {
                _grid.Add(cellHash, new List<CircleCollider>(8));
            }

            // Add collider if it is not already there
            if (!ColliderExists(cellHash, collider))
            {
                _grid[cellHash].Add(collider);
                SaveEntityCellCoords(collider.Entity.Id, cellHash);
            }
        }

        private void SaveEntityCellCoords(uint id, long cellHash)
        {
            if (!_entityCoordinates.ContainsKey(id))
            {
                _entityCoordinates.Add(id, new List<long>());
            }

            _entityCoordinates[id].Add(cellHash);
        }

        public void RemoveCollider(CircleCollider collider)
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

        private bool ColliderExists(long cellHash, CircleCollider circleCollider)
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
