using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Zand;
using Zand.AI;
using Zand.Colliders;
using Zand.Components;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    class BaseUnitState : State<Unit>
    {
        protected CommandQueue _unitCommands;
        protected Mover _mover;
        protected Animator _animator;
        protected SoundEffectManager _sfxManger;
        protected UnitCommand CurrentCommand;

        public override void OnInitialize()
        {
            _unitCommands = _context.Entity.GetComponent<CommandQueue>(false);
            _mover = _context.Entity.GetComponent<Mover>(false);
            _animator = _context.Entity.GetComponent<Animator>(false);
            _sfxManger = _context.Entity.Scene.GetComponent<SoundEffectManager>();
        }

        public override void Update()
        {
            CurrentCommand = _unitCommands.Peek();
        }

        protected bool TargetsAreNearby(out Entity entity)
        {
            var entitiesInRange = _context.Scene.Physics.GetEntitiesWithin(_context.Entity.Position, _context.SightRange);
            entity = GetClosestEnemyUnit(entitiesInRange);
            return entity != null;
        }

        protected bool TargetIsInRange(Entity target, float buffer = 0)
        {
            var distanceSquared = Vector2.DistanceSquared(_context.Entity.Position, target.Position);
            var attackRangeSquared = Math.Pow(_context.AttackRange, 2d);

            return attackRangeSquared - distanceSquared > buffer;
        }

        protected Entity GetClosestEnemyUnit(List<Entity> entities)
        {
            var shortestDistance = float.MaxValue;
            Entity closestEnemy = null;

            foreach (var entity in entities)
            {
                var allegiance = entity.GetComponent<UnitAllegiance>();
                bool sameTeam = allegiance.Id == _context.Entity.GetComponent<UnitAllegiance>().Id;

                if (entity.IsDestroyed || sameTeam || entity.Name != "unit")
                {
                    continue;
                }

                float distance = Vector2.Distance(entity.Position, _context.Entity.Position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestEnemy = entity;
                }
            }

            return closestEnemy;
        }

        protected void SetUnitAnimation(string animationVerb)
        {
            string animation = animationVerb + _mover.Orientation.ToString();
            _animator.Play(animation);
        }

        protected void DetermineState(UnitCommand cmd)
        {
            switch (cmd.Type)
            {
                case CommandType.Move:
                case CommandType.AttackMove:
                    _machine.ChangeState<Moving>();
                    break;
                case CommandType.Attack:
                    _machine.ChangeState<Attacking>();
                    break;
                case CommandType.UseAbility:
                    _machine.ChangeState<ExecuteAbility>();
                    break;
                default:
                    throw new NotImplementedException($"Unknown Command: {CurrentCommand}");
            }
        }

        protected Vector2 GetUnitVelocity(Entity entity)
        {
            var units = _context.Entity.Scene.Physics.GetCollidersWithin(_context.Entity.ScreenPosition, BoidConfig.BoidVision);
            var (friendlies, statics) = SortNearbyUnits(units);
            var unitCollider = _context.Entity.GetComponent<CircleCollider>();

            //var cohesionV = GetCohesionVelocity(unitCollider, friendlies);
            //var seperationV = GetSeperationVelocity(unitCollider, friendlies);
            //var groupV = GetGroupVelocity(unitCollider, friendlies);
            var destinationV = GetDestinationVelocity(unitCollider);
            var avoidV = GetAvoidVelocity(unitCollider, statics);


            var unitVelocity =  destinationV + avoidV; //cohesionV + seperationV + groupV +
            unitVelocity = ClampVelocity(unitVelocity, _mover.MaxSpeed);

            // TODO arrival checks?


            return unitVelocity;
        }


        private (List<CircleCollider> friendlies, List<CircleCollider> statics) SortNearbyUnits(List<CircleCollider> colliders)
        {
            // we are assuming a circle collider means it is a unit ...
            var statics = new List<CircleCollider>();
            var friendlies = new List<CircleCollider>();

            var unitAllegiance = _context.Entity.GetComponent<UnitAllegiance>();

            for (int i = 0; i < colliders.Count; i++)
            {
                if (colliders[i].Static)
                {
                    statics.Add(colliders[i]);
                }

                if (colliders[i].Entity.GetComponent<UnitAllegiance>().Id == unitAllegiance.Id && UnitHasSaveMoveCommand(colliders[i]))
                {
                    friendlies.Add(colliders[i]);
                }
            }

            return (friendlies, statics);
        }

        private bool UnitHasSaveMoveCommand(CircleCollider unitCollider)
        {
            return unitCollider.Entity.GetComponent<CommandQueue>().Peek()?.Destination.Position == CurrentCommand.Destination.Position;
        }


        private Vector2 GetCohesionVelocity(CircleCollider unitCollider, List<CircleCollider> colliders)
        {
            Vector2 center = Vector2.Zero;
            foreach (CircleCollider c in colliders)
            {
                center += c.Center;
            }

            center /= colliders.Count;
            return (center - unitCollider.Center) * Config.CohesionFactor;
        }

        private Vector2 GetSeperationVelocity(CircleCollider unitCollider, List<CircleCollider> colliders)
        {
            Vector2 seperationVelocity = Vector2.Zero;

            foreach (CircleCollider c in colliders)
            {
                if (c.Entity.Id != unitCollider.Entity.Id)
                {
                    var distance = Vector2.Distance(unitCollider.Center, c.Center);
                    var avoidDistance = c.Radius + unitCollider.Radius + BoidConfig.SeperationMinDistance;

                    if (distance < avoidDistance)
                    {
                        seperationVelocity += (unitCollider.Center - c.Center);
                    }
                }
            }

            return seperationVelocity * BoidConfig.SeperationFactor;
        }

        private Vector2 GetGroupVelocity(CircleCollider unitCollider, List<CircleCollider> colliders)
        {
            Vector2 averageVelocity = Vector2.Zero;
            int count = 0;
            foreach (CircleCollider c in colliders)
            {
                if (Vector2.DistanceSquared(unitCollider.Center, c.Center) < BoidConfig.BoidVisionSquared)
                {
                    averageVelocity += c.Entity.GetComponent<Mover>().Velocity;
                    count++;
                }

            }

            averageVelocity /= count;
            return (averageVelocity - _mover.Velocity) * BoidConfig.GroupAlignmentFactor;
        }

        private Vector2 GetDestinationVelocity(CircleCollider unit)
        {
            var destVelocity = Vector2.Zero;
            if (CurrentCommand.Type == CommandType.Move)
            {
                destVelocity = CurrentCommand.Destination.Position - unit.Center;
                destVelocity.Normalize();
                destVelocity *= BoidConfig.MaxSpeed;
            }
            return destVelocity * BoidConfig.DestinationFactor;
        }

        private Vector2 GetAvoidVelocity(CircleCollider unitCollider, List<CircleCollider> unitColliders)
        {
            Vector2 avoidVelocity = Vector2.Zero;

            foreach (CircleCollider c in unitColliders)
            {
                var distance = Vector2.DistanceSquared(unitCollider.Center, c.Center);
                var avoidDistance = c.Radius + unitCollider.Radius + BoidConfig.AvoidOtherMinDist;
                avoidDistance = avoidDistance * avoidDistance;

                if (distance < avoidDistance)
                {
                    avoidVelocity += (unitCollider.Center - c.Center);
                }
            }
            // make the avoid velocity taper off depeding on the distance?
            return avoidVelocity * BoidConfig.AvoidOtherFactor;
        }

        private Vector2 ClampVelocity(Vector2 velocity, float maxSpeed)
        {
            if (velocity.Length() > maxSpeed)
            {
                velocity.Normalize();
                velocity *= maxSpeed;
            }
            return velocity;
        }
    }
}
