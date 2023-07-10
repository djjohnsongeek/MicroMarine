using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand;
using Zand.AI;
using Zand.Components;

namespace MicroMarine.Components
{
    class BaseMarineState : State<Unit>
    {
        protected Entity SearchForTarget()
        {
            var entitiesInRange = _context.Scene.Physics.GetEntitiesWithin(_context.Entity.Position, _context.AttackRange);
            var testDistance = float.MaxValue;
            Entity newTarget = null;

            foreach (var entity in entitiesInRange)
            {
                if (entity.IsDestroyed)
                {
                    continue;
                }

                if (entity.Name == "marine")
                {
                    var allegiance = entity.GetComponent<UnitAllegiance>();

                    if (allegiance.Id != _context.Entity.GetComponent<UnitAllegiance>().Id)
                    {
                        float distance = Vector2.Distance(entity.Position, _context.Entity.Position);
                        if (distance < testDistance)
                        {
                            testDistance = distance;
                            newTarget = entity;
                        }
                    }
                }
            }

            return newTarget;
        }
    }
}
