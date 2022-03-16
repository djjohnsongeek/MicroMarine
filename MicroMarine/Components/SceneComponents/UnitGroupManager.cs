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

        public UnitGroupManager(Scene scene) : base(scene)
        {
            UnitGroups = new SortedList<uint, UnitGroup>();
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
            }

            for (uint i = 0; i < UnitGroups.Count; i++ )
            {
                UnitGroups[i].Update();
            }
        }
    }

    public class UnitGroup
    {
        public List<Entity> Units;
        public uint Id;
        public Queue<Vector2> Waypoints;
        private static int _dissensionFactor = 100;
        private static int _baseUnitSpeed = 100;

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
            var (centerOfMass, neighborVelocity) = GetVelocities();
            for (int i = 0; i < Units.Count; i++)
            {
                Vector2 cohesionVelocity = GetCohesionVelocity(Units[i], centerOfMass);
                Vector2 avoidanceVelocity = GetAvoidanceVelocity(Units[i]);
                Vector2 neighborVelocity = GetNeighborVelocity(Units[i]);
                Vector2 destinationVelocity = GetDestinationVelocity(Units[i]);
            }
        }

        private (Vector2 centerOfMass, Vector2 neighborV) GetVelocities()
        {
            var centerOfMass = Vector2.Zero;
            var neighborV = Vector2.Zero;

            for (int i = 0; i < Units.Count; i++)
            {
                centerOfMass += Units[i].ScreenPosition;
                neighborV += Units[i].GetComponent<WaypointMovement>().Velocity;
            }

            return (Vector2.Divide(centerOfMass, Units.Count), neighborV);
        }

        private Vector2 GetCohesionVelocity(Entity unit, Vector2 centerOfMass)
        {
            return Vector2.Divide((centerOfMass - unit.ScreenPosition), _dissensionFactor);
        }

        private Vector2 GetAvoidanceVelocity(Entity unit)
        {
            return Vector2.Zero;
        }

        private Vector2 GetNeighborVelocity(Entity unit)
        {
            return Vector2.Zero;
            for (int i = 0; i < Units.Count; i ++)
            // average group velocity
        }

        private Vector2 GetDestinationVelocity(Entity unit)
        {
            return Vector2.Zero;
        }
    }
}
