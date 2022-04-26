using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zand.AI;
using Microsoft.Xna.Framework;
using Zand.ECS.Components;
using Zand;

namespace MicroMarine.Components.UnitGroups
{
    internal class Grouping : State<UnitGroup>
    {
        public override void Enter()
        {
            _context.StopDistance = _context.GetStopDistance();
            _context.SetUnitsToRunning();
            _context.SetUnitStatic(_context.Leader);
        }

        public override void Exit()
        {
            _context.RemoveStatic(_context.Leader);
            _context.GroupingClock = 0;
        }

        public override void Update()
        {
            int unitsGrouping = 0;
            if (_context.Waypoints.Count > 0)
            {
                _machine.ChangeState<Moving>();
            }

            for (int i = 0; i < _context.Units.Count; i++)
            {
                // Skips units who have arrived
                if (_context.Units[i].GetComponent<UnitState>().CurrentState == UnitStates.Idle || _context.Units[i].Id == _context.Leader.Id)
                {
                    continue;
                }

                float distanceToLeader = Vector2.Distance(_context.Leader.Position, _context.Units[i].Position);
                if (_context.ShouldGroup(distanceToLeader))
                {
                    _context.Units[i].GetComponent<Mover>().Velocity = _context.GetGroupingVelocity(_context.Units[i]);
                    unitsGrouping++;
                }
                else
                {
                    _context.Units[i].GetComponent<Mover>().Velocity = Vector2.Zero;
                    _context.Units[i].GetComponent<UnitState>().CurrentState = UnitStates.Idle;
                }
            }

            if (unitsGrouping == 0)
            {
                _machine.ChangeState<Idle>();
            }

            _context.GroupingClock += (float)Time.DeltaTime;
        }
    }
}
