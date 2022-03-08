﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.ECS.Components;

namespace Zand.Physics
{
    class SpatialHash
    {

        // use world cordinates, add remove as needed (not for static units)
        // JIT init?
        private Dictionary<int, List<CircleCollider>> _buckets;

        private int _cols;
        private int _rows;
        private double _conversionFactor;
        private Scene _scene;

        public SpatialHash(Scene scene, int cellSize)
        {
            _scene = scene;
            _cols = _scene.ScreenWidth / cellSize;
            _rows = _scene.ScreenHeight / cellSize;
            _conversionFactor = 1d / cellSize;
            _buckets = new Dictionary<int, List<CircleCollider>>(_cols * _rows);
            InitBuckets();
        }

        public IReadOnlyCollection<CircleCollider> GetNearby(Vector2 position)
        {
            int key = GetBucketKey(position);
            if (IsKeyOutOfRange(key))
            {
                return new Collection<CircleCollider>();
            }

            return _buckets[GetBucketKey(position)].AsReadOnly();
        }

        public void AddCollider(CircleCollider collider)
        {
            Vector2 screenPos = collider.Center;
            Vector2 bottomRight = new Vector2(screenPos.X + collider.Radius, screenPos.Y + collider.Radius);
            Vector2 topLeft = new Vector2(screenPos.X - collider.Radius, screenPos.Y - collider.Radius);
            Vector2 topRight = new Vector2(bottomRight.X, topLeft.Y);
            Vector2 bottomLeft = new Vector2(topLeft.X, bottomRight.Y);

            AddToBucket(GetBucketKey(bottomRight), collider);
            AddToBucket(GetBucketKey(topLeft), collider);
            AddToBucket(GetBucketKey(topRight), collider);
            AddToBucket(GetBucketKey(bottomLeft), collider);
        }

        public void Reset()
        {
            _buckets.Clear();
            InitBuckets();
        }

        private void AddToBucket(int key, CircleCollider collider)
        {
            if (IsKeyOutOfRange(key))
            {
                return;
            }

            if (!ExistsInBucket(key, collider))
            {
                _buckets[key].Add(collider);
            }
        }

        private bool ExistsInBucket(int key, CircleCollider circleCollider)
        {
            if (IsKeyOutOfRange(key))
            {
                return false;
            }

            foreach (var collider in _buckets[key])
            {
                if (collider == circleCollider)
                {
                    return true;
                }
            }

            return false;
        }

        private int GetBucketKey(Vector2 vector)
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

        private bool IsKeyOutOfRange(int k)
        {
            return k < 0 || k > _cols * _rows - 1;
        }
    }


}
