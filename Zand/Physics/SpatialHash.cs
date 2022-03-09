using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.ECS.Components;

namespace Zand.Physics
{
    // hash by world position (so that it is not constantly changing)
    // only update the collisions that are moving (those that are still need no update)
    // will need to manually remove from its old buckts

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
            long cellHash = GetCallHash(position);
            if (!_grid.ContainsKey(cellHash))
            {
                return new Collection<CircleCollider>();
            }
            return _grid[GetCallHash(position)].AsReadOnly();
        }

        public void AddCollider(CircleCollider collider)
        {
            Vector2 bottomRight = new Vector2(collider.Center.X + collider.Radius, collider.Center.Y + collider.Radius);
            Vector2 topLeft = new Vector2(collider.Center.X - collider.Radius, collider.Center.Y - collider.Radius);
            Vector2 topRight = new Vector2(bottomRight.X, topLeft.Y);
            Vector2 bottomLeft = new Vector2(topLeft.X, bottomRight.Y);

            AddToCell(GetCallHash(bottomRight), collider);
            AddToCell(GetCallHash(topLeft), collider);
            AddToCell(GetCallHash(topRight), collider);
            AddToCell(GetCallHash(bottomLeft), collider);
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

            // Remove from previous positions
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

        private long GetCallHash(Vector2 vector)
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
