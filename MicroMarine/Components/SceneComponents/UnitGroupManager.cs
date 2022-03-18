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
            List<Entity> units = Scene.GetComponent<UnitSelector>().GetSelectedUnits();
            var group = new UnitGroup(units, Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));
            group._scene = Scene;
            UnitGroups.Remove(group.Id);
            UnitGroups.Add(group.Id, group);
            GroupIds.Add(group.Id);
        }
    }

    public class UnitGroup
    {
        // JUST FOR DEBUG
        public Scene _scene;
        public int i = 0;


        public uint Id;
        public List<Entity> Units;
        public Queue<Vector2> Waypoints;
        public Vector2? CurrentWaypoint;

        private static float _cohesionFactor = .05f;
        private static float _avoidFactor = .05f;
        private static int _maxDistanceSqrd = 1296; // 36 * 36
        private static float _unitSpeed = .1F;
        private static float _arrivalThreshold = 150;
        private static float _destinationFactor = 100F;

        public UnitGroup(List<Entity> units, Vector2 destination)
        {
            Waypoints = new Queue<Vector2>();
            Waypoints.Enqueue(destination);
            CurrentWaypoint = null;
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
            i++;
            if (CurrentWaypoint == null && Waypoints.Count > 0)
            {
                CurrentWaypoint = Waypoints.Dequeue();
            }
            else if(CurrentWaypoint == null && Waypoints.Count == 0)
            {
                return;
            }

            var (centerOfMass, neighborVelocity) = GetNeigborVelocity(); // neightbor velocity is SOOO large
            var groupDistance = Vector2.DistanceSquared(centerOfMass, CurrentWaypoint.Value);

            for (int i = 0; i < Units.Count; i++)
            {

                // Vector2 cohesionVelocity = GetCohesionVelocity(Units[i], centerOfMass);
                Vector2 avoidanceVelocity = GetAvoidanceVelocity(Units[i]);
                Vector2 destinationVelocity = GetDestinationVelocity(Units[i]);

                var finalV = destinationVelocity + avoidanceVelocity;

                if (groupDistance <= _arrivalThreshold)
                {
                    finalV = Vector2.Zero;
                    CurrentWaypoint = null;
                }

                Units[i].GetComponent<Mover>().Velocity = finalV;
            }

            //_scene.Debug.Log($"{i}Center Of Mass -  X: {centerOfMass.X} Y: {centerOfMass.Y}");
            //_scene.Debug.Log($"{i}Destination -  X: {CurrentWaypoint.Value.X} Y: {CurrentWaypoint.Value.Y}");
            //_scene.Debug.Log($"{i}Distance Squared Diff = {distance}");
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
            if (CurrentWaypoint == null)
            {
                return Vector2.Zero;
            }

            var destinationV = CurrentWaypoint.Value - unit.Position;
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
