using Microsoft.Xna.Framework;
using System;
using Zand;
using Zand.AI;
using Zand.Components;
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

        }

        public override void Enter()
        {
            var currentCommand = GetCommand();
            for (int i = 0; i < _context.Units.Count; i++)
            {
                CommandQueue unitCommands = _context.Units[i].GetComponent<CommandQueue>();
                unitCommands.AddCommand(currentCommand);
            }
            currentCommand.SetStatus(CommandStatus.Issued);
        }

        public override void Update()
        {
            var currentCommand = GetCommand();
            if (currentCommand == null)
            {
                _machine.ChangeState<Idle>();
                return;
            }

            //// Ensure we have a destination
            ////if (_context.CommandQueue.CurrentCommand == null)
            ////{
            ////    _context.CurrentWaypoint = _context.Waypoints.Dequeue();
            ////}

            //Vector2 centerOfMass = _context.GetCenterOfMass();
            //float leaderDistance = Vector2.DistanceSquared(_context.Leader.Position, _context.GroupCommands.CurrentCommand.TargetLocation);

            //for (int i = 0; i < _context.Units.Count; i++)
            //{
            //    // Skip units who have "arrived"

            //    // Gather velocities and distances
            //    var unitVelocity = _context.GetDestinationVelocity(_context.Units[i]) + _context.GetCohesionVelocity(_context.Units[i], centerOfMass);

            //    // Check if units has arrived
            //    if (leaderDistance <= Config.ArrivalThreshold)
            //    {
            //        unitVelocity = Vector2.Zero;
            //    }

            //    // Update unit's velocity
            //    _context.Units[i].GetComponent<Mover>().Velocity = unitVelocity;
            //}
        }

        public UnitCommand GetCommand()
        {
            if (_context.GroupCommands.CurrentCommand is null || _context.GroupCommands.CurrentCommand.Status == CommandStatus.Completed)
            {
                _context.GroupCommands.Next();
            }

            return _context.GroupCommands.CurrentCommand;
        }
    }
}
