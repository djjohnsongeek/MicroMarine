using Microsoft.Xna.Framework;
using Zand;
using Zand.AI;
using Zand.ECS.Components;

namespace MicroMarine.Components.UnitGroups
{
    public class Moving : State<UnitGroup>
    {
        public override void OnInitialize()
        {

        }

        public override void Exit()
        {
            _context.CurrentWaypoint = null;
        }

        public override void Enter()
        {
            _context.SetUnitsToRunning();
            if (_context.CurrentWaypoint == null)
            {
                _context.CurrentWaypoint = _context.Waypoints.Dequeue();
            }
        }

        public override void Update()
        {
            // Ensure we have a destination
            if (_context.CurrentWaypoint == null)
            {
                _context.CurrentWaypoint = _context.Waypoints.Dequeue();
            }

            Vector2 centerOfMass = _context.GetCenterOfMass();
            float leaderDistance = Vector2.DistanceSquared(_context.Leader.Position, _context.CurrentWaypoint.Value);
            int unitsInMotion = 0;

            for (int i = 0; i < _context.Units.Count; i++)
            {
                // Skip units who have "arrived"
                UnitState unitState = _context.Units[i].GetComponent<UnitState>();
                if (unitState.CurrentState == UnitStates.Idle)
                {
                    continue;
                }

                // Gather velocities and distances
                var unitVelocity = _context.GetDestinationVelocity(_context.Units[i]) + _context.GetCohesionVelocity(_context.Units[i], centerOfMass);

                // Check if units has arrived
                if (leaderDistance <= Config.ArrivalThreshold)
                {
                    unitVelocity = Vector2.Zero;
                    unitState.CurrentState = UnitStates.Idle;
                }

                // Update unit's velocity
                _context.Units[i].GetComponent<Mover>().Velocity = unitVelocity;

                // Track number of units currently in motion
                unitsInMotion++;
            }

            // All units have arrived
            if (unitsInMotion == 0)
            {
                // Follow the next waypoint
                if (_context.Waypoints.Count > 0)
                {
                    _context.CurrentWaypoint = _context.Waypoints.Dequeue();
                    _context.SetUnitsToRunning();
                }
                // Group up
                else
                {
                    _machine.ChangeState<Grouping>();
                }
            }
        }
    }
}
