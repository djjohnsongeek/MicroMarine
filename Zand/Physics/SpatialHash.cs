using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.ECS.Components;

namespace Zand.Physics
{
    class SpatialHash
    {
        private Dictionary<int, List<CircleCollider>> _buckets;

        private int _cols;
        private int _rows;
        private double _conversionFactor;

        public SpatialHash(int screenWidth, int screenHeight, int cellSize)
        {
            _cols = screenWidth / cellSize;
            _rows = screenHeight / cellSize;
            _conversionFactor = 1d / cellSize;
            _buckets = new Dictionary<int, List<CircleCollider>>(_cols * _rows);
            InitBuckets();
        }

        public IReadOnlyCollection<CircleCollider> GetNearby(Vector2 position)
        {
            return _buckets[BucketKey(position)].AsReadOnly();
        }

        public void Clear()
        {
            _buckets.Clear();
        }

        public void AddCollider(CircleCollider collider)
        {
            Vector2 screenPos = collider.Entity.ScreenPosition;
            Vector2 bottomRight = new Vector2(screenPos.X + collider.Radius, screenPos.Y + collider.Radius);
            Vector2 topLeft = new Vector2(screenPos.X - collider.Radius, screenPos.Y - collider.Radius);
            Vector2 topRight = new Vector2(bottomRight.X, topLeft.Y);
            Vector2 bottomLeft = new Vector2(topLeft.X, bottomRight.Y);

            AddToBucket(BucketKey(bottomRight), collider);
            AddToBucket(BucketKey(topLeft), collider);
            AddToBucket(BucketKey(topRight), collider);
            AddToBucket(BucketKey(bottomLeft), collider);
        }

        private void AddToBucket(int key, CircleCollider collider)
        {
            if (!ExistsInBucket(key, collider))
            {
                _buckets[key].Add(collider);
            }
        }

        private bool ExistsInBucket(int index, CircleCollider circleCollider)
        {
            foreach (var collider in _buckets[index])
            {
                if (collider == circleCollider)
                {
                    return true;
                }
            }

            return false;
        }

        private int BucketKey(Vector2 vector)
        {
            double key =  Math.Floor(vector.X * _conversionFactor) + Math.Floor(vector.Y * _conversionFactor) * _cols;
            return (int)key;
        }

        private void InitBuckets()
        {
            for (int i = 0; i < _cols * _rows; i++)
            {
                _buckets.Add(i, new List<CircleCollider>(8));
            }
        }
    }


}
