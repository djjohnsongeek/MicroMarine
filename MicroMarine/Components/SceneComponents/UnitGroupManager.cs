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
        private uint IdPool;

        public UnitGroupManager(Scene scene) : base(scene)
        {
            UnitGroups = new SortedList<uint, UnitGroup>();
            GroupIds = new List<uint>();
            IdPool = 0;

            // TODO initialize a unit group "pool"
        }

        public override void Update()
        {
            // TODO remove stale unit groups

            if (Input.RightMouseWasPressed())
            {
                CreateNewUnitGroup();
            }

            for (int i = 0; i < GroupIds.Count; i++ )
            {
                UnitGroups[GroupIds[i]].Update();
            }
        }

        private void CreateNewUnitGroup()
        {
            // TODO remove units in this group from all other groups
            List<Entity> units = Scene.GetComponent<UnitSelector>().GetSelectedUnits();
            Vector2 destination = Scene.Camera.GetWorldLocation(Input.MouseScreenPosition);
            RegisterGroup(new UnitGroup(units, destination));
        }

        private void RegisterGroup(UnitGroup group)
        {
            AssignId(group);
            group._scene = Scene;
            StealUnits(group);
            UnitGroups.Add(group.Id, group);
            GroupIds.Add(group.Id);
        }

        private void StealUnits(UnitGroup group)
        {
            // nieve implementation
            for (int i = 0; i < group.Units.Count; i ++)
            {
                for (int j = 0; j < UnitGroups.Count; j++)
                {
                    if (UnitGroups.Values[j].Units.Contains(group.Units[i]))
                    {
                        UnitGroups.Values[j].Units.Remove(group.Units[i]);
                        UnitGroups.Values[j].AssignNewLeader();
                        break;
                    }
                }
            }
        }

        private void AssignId(UnitGroup group)
        {
            IdPool++;
            group.Id = IdPool;
        }
    }

    public class UnitGroup
    {
        // JUST FOR DEBUG
        public Scene _scene;


        public uint Id;
        public List<Entity> Units;
        public Entity Leader = null;
        public Queue<Vector2> Waypoints;
        public Vector2? CurrentWaypoint;

        private static float _matchFactor = 0.125f;
        private static float _cohesionFactor = .2f;
        private static float _avoidFactor = .8f; // .5
        private static int _maxDistanceSqrd = 25 * 25;
        private static float _unitSpeed = .1F;
        private static float _arrivalThreshold = 1;
        private static float _destinationFactor = 100F;

        public UnitGroup(List<Entity> units, Vector2 destination)
        {
            // TODO: use a pool?
            Waypoints = new Queue<Vector2>();
            Waypoints.Enqueue(destination);
            CurrentWaypoint = null;
            InitGroup(units);
        }

        private void InitGroup(List<Entity> units)
        {
            // Init properties
            Units = units;
            Leader = null;

            AssignNewLeader();
        }

        public void AssignNewLeader()
        {
            Vector2 centerOfMass = GetCenterOfMass();
            float distance = float.MaxValue;

            for (int i = 0; i < Units.Count; i++)
            {
                float unitDistance = Vector2.Distance(Units[i].Position, centerOfMass);
                if (unitDistance < distance)
                {
                    Leader = Units[i];
                    distance = unitDistance;
                }
            }
        }

        public void Update()
        {
            if (CurrentWaypoint == null && Waypoints.Count > 0)
            {
                CurrentWaypoint = Waypoints.Dequeue();
            }
            else if (CurrentWaypoint == null && Waypoints.Count == 0)
            {
                return;
            }

            Vector2 centerOfMass = GetCenterOfMass();
            var leaderDistance = Vector2.DistanceSquared(Leader.Position, CurrentWaypoint.Value);

            for (int i = 0; i < Units.Count; i++)
            {

                Vector2 cohesionVelocity = GetCohesionVelocity(Units[i], centerOfMass);
                Vector2 destinationVelocity = GetDestinationVelocity(Units[i]);

                var finalV = destinationVelocity + cohesionVelocity; // + avoidanceVelocity, neighborVelocity;

                // var distance = Vector2.DistanceSquared(centerOfMass, CurrentWaypoint.Value);

                if (leaderDistance <= _arrivalThreshold)
                {
                    finalV = Vector2.Zero;
                    CurrentWaypoint = null;
                    // after arriving, if there is no other waypoint "collapse" (basically perform chohesion only)
                }

                Units[i].GetComponent<Mover>().Velocity = finalV;
            }

            //_scene.Debug.Log($"{i}Center Of Mass -  X: {centerOfMass.X} Y: {centerOfMass.Y}");
            //_scene.Debug.Log($"{i}Destination -  X: {CurrentWaypoint.Value.X} Y: {CurrentWaypoint.Value.Y}");
            //_scene.Debug.Log($"{i}Distance Squared Diff = {distance}");
        }

        private Vector2 GetCenterOfMass()
        {
            var center = Vector2.Zero;
            for (int i = 0; i < Units.Count; i++)
            {
                center += Units[i].Position;
            }
            return Vector2.Divide(center, Units.Count);
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
                    if (distanceSqrd < _maxDistanceSqrd)
                    {
                        avoidV = avoidV - (unitPos - unitIPos);
                    }
                }
            }

            return avoidV;
        }

        private Vector2 GetDestinationVelocity(Entity unit)
        {
            if (CurrentWaypoint == null)
            {
                return Vector2.Zero;
            }

            var destinationV = CurrentWaypoint.Value - Leader.Position; // unit.Position
            return Vector2.Normalize(destinationV) * _destinationFactor;
        }

        private Vector2 LimitVelocity(Vector2 currentVelocity)
        {
            if (currentVelocity.Length() > _unitSpeed)
            {
                return Vector2.Normalize(currentVelocity) * _unitSpeed;
            }

            return currentVelocity;
        }
    }
}
