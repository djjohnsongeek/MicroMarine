using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Zand.Colliders;
using Zand.ECS.Components;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Zand.Physics
{
    class SpatialHash
    {
        private Dictionary<long, List<Collider>> _grid;
        private Dictionary<int, List<long>> _entityCoordinates;
        private double _conversionFactor;
        private int _cellSize;

        public SpatialHash(int cellSize)
        {
            _conversionFactor = 1d / cellSize;
            _cellSize = cellSize;
            _grid = new Dictionary<long, List<Collider>>(1000);
            _entityCoordinates = new Dictionary<int, List<long>>();
        }

        public IReadOnlyCollection<Collider> GetNearby(Vector2 position)
        {
            long cellHash = GetCellHash(position);
            if (!CellExists(cellHash))
            {
                return new Collection<Collider>();
            }
            return _grid[cellHash];
        }

        public IReadOnlyCollection<Collider> GetWithin(RectangleF boundingBox)
        {
            HashSet<Collider> colliders = new HashSet<Collider>();

            // too much instantiation....
            var topLeft = new Point(GetCellAxis(boundingBox.X), GetCellAxis(boundingBox.Y));
            var bottomRight = new Point(GetCellAxis(boundingBox.Right), GetCellAxis(boundingBox.Bottom));

            for (int x = topLeft.X; x <= bottomRight.X; x++)
            {
                for (int y = topLeft.Y; y <= bottomRight.Y; y++)
                {
                    var cellCoords = new Point(x, y);
                    var key = HashCoords(cellCoords);

                    if (CellExists(key))
                    {
                        foreach (var collider in _grid[key])
                        {
                            colliders.Add(collider);
                        }
                    }

                }
            }

            return colliders;
        }

        public List<CircleCollider> GetWithin(Vector2 position, float distance)
        {
            var boundingCircle = new Circle(position, distance);
            var boundingBox = new RectangleF(position.X - distance, position.Y - distance, distance * 2, distance * 2);


            var colliders = new List<CircleCollider>();
            var possibleColliders = GetWithin(boundingBox);

            foreach (var collider in possibleColliders)
            {
                if (collider is CircleCollider c)
                {
                    if (Collisions.CircleOverLaps(boundingCircle, c))
                    {
                        colliders.Add(c);
                    }
                }
            }

            return colliders;
        }

        public void AddCollider(Collider collider)
        {
            for (float x = collider.TopLeft.X; x <= collider.BottomRight.X; x++)
            {
                for (float y = collider.TopLeft.Y; y <= collider.BottomRight.Y; y++)
                {
                    AddToCell(GetCellHash(new Vector2(x, y)), collider);
                }
            }

            collider.Dirty = false;
        }

        private void AddToCell(long cellHash, Collider collider)
        {
            // Create New Cell if none exists
            if (!CellExists(cellHash))
            {
                _grid.Add(cellHash, new List<Collider>(8));
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

        public void RemoveCollider(Collider collider)
        {
            // No need to remove if it's not there
            if (!_entityCoordinates.ContainsKey(collider.Entity.Id))
            {
                return;
            }

            // Remove from previous cells
            foreach (var cellCoord in _entityCoordinates[collider.Entity.Id])
            {
                var list = _grid[cellCoord];
                var index = DetermineColliderIndex(list, collider);
                 _grid[cellCoord].RemoveAt(index);
            }

            // Remove this entities previous cell coords
            _entityCoordinates[collider.Entity.Id].Clear();
        }

        private int DetermineColliderIndex(List<Collider> colliders, Collider collider)
        {
            for (int i = 0; i < colliders.Count; i++)
            {
                if (colliders[i].Entity.Id == collider.Entity.Id)
                {
                    return i;
                }
            }

            return -1;
        }

        private bool ColliderExists(long cellHash, Collider circleCollider)
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

            return HashCoords(new Point(x, y));
        }

        private long HashCoords(Point cellCoords)
        {
            return (long)cellCoords.X << 32 | (long)(uint)cellCoords.Y;
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
