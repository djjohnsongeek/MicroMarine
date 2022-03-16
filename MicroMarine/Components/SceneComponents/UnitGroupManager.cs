using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    public class UnitGroupManager : SceneComponent
    {
        private SortedList<uint, UnitGroup> UnitGroups;
        private List<uint> GroupIds;

        public UnitGroupManager(Scene scene) : base(scene)
        {
            UnitGroups = new SortedList<uint, UnitGroup>();
            GroupIds = new List<uint>();
        }

        public override void Update()
        {
            // Remove 'stale' units groups
            // TODO

            // Add new unit groups
            if (Input.RightMouseWasPressed())
            {
                // Add Units Groups
                var unitSelector = Scene.GetComponent<UnitSelector>();

                // TODO Replace group if it already exists
                var group = new UnitGroup(unitSelector.GetSelectedUnits(), Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));
                UnitGroups.Add(group.Id, group);
                GroupIds.Add(group.Id);
            }

            for (int i = 0; i < GroupIds.Count; i++ )
            {
                UnitGroups[GroupIds[i]].Update();
            }
        }
    }

    public class UnitGroup
    {
        public List<Entity> Units;
        public uint Id;
        public Queue<Vector2> Waypoints;

        private static float _cohesionFactor = .05f;
        private static float _avoidFactor = .05f;
        private static int _maxDistanceSqrd = 1296; // 36 * 36
        private static float _unitSpeed = 200;

        public UnitGroup(List<Entity> units, Vector2 destination)
        {
            Waypoints = new Queue<Vector2>();
            Waypoints.Enqueue(destination);
            Units = units;
            SetGroupId();
        }

        private void SetGroupId()
        {
            for (int i = 0; i < Units.Count; i++)
            {
                Id += Units[i].Id;
            }
        }

        public void Update()
        {
            var (centerOfMass, neighborVelocity) = GetNeigborVelocity();
            for (int i = 0; i < Units.Count; i++)
            {
                Vector2 cohesionVelocity = GetCohesionVelocity(Units[i], centerOfMass);
                Vector2 avoidanceVelocity = GetAvoidanceVelocity(Units[i]);
                Vector2 destinationVelocity = GetDestinationVelocity(Units[i]);

                Units[i].GetComponent<Mover>().Velocity += cohesionVelocity + neighborVelocity + avoidanceVelocity + destinationVelocity;
            }
        }

        private (Vector2 centerOfMass, Vector2 neighborV) GetNeigborVelocity()
        {
            var centerOfMass = Vector2.Zero;
            var neighborV = Vector2.Zero;

            for (int i = 0; i < Units.Count; i++)
            {
                centerOfMass += Units[i].GetComponent<CircleCollider>().Center;
                neighborV += Units[i].GetComponent<Mover>().Velocity;
            }

            neighborV = Vector2.Divide(neighborV, Units.Count);
            centerOfMass = Vector2.Divide(centerOfMass, Units.Count);

            return (centerOfMass, neighborV);
        }

        private Vector2 GetCohesionVelocity(Entity unit, Vector2 centerOfMass)
        {
            var unitPos = unit.GetComponent<CircleCollider>().Center;
            return (centerOfMass - unitPos) * _cohesionFactor;
        }

        private Vector2 GetAvoidanceVelocity(Entity unit)
        {
            var avoidV = Vector2.Zero;
            Vector2 unitPos = unit.GetComponent<CircleCollider>().Center;
            for (int i = 0; i < Units.Count; i++)
            {
                Vector2 unitIPos = Units[i].GetComponent<CircleCollider>().Center;
                if (unit != Units[i])
                {
                    float distanceSqrd = Vector2.DistanceSquared(unitPos, unitIPos);
                    if (distanceSqrd < (_maxDistanceSqrd))
                    {
                        avoidV -= (unitPos - unitIPos);
                    }
                }
            }

            return avoidV * _avoidFactor;
        }

        private Vector2 GetDestinationVelocity(Entity unit)
        {
            var destinationV = unit.GetComponent<CircleCollider>().Center - Waypoints.Peek();
            destinationV.Normalize();
            return destinationV * ((float)Time.DeltaTime * _unitSpeed);
        }
    }
}
