using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;
using MicroMarine.Components.UnitGroups;
using Zand.Utils;
using System.Collections;
using MicroMarine.Extensions;

namespace MicroMarine.Components
{
    public class UnitGroupManager : SceneComponent
    {
        private List<UnitGroup> UnitGroups;
        private List<UnitGroup> AffectedGroups;
        private Pool<UnitGroup> _unitGroupPool;

        public UnitGroupManager(Scene scene) : base(scene)
        {
            _unitGroupPool = new Pool<UnitGroup>(100);
            UnitGroups = new List<UnitGroup>(10);
            AffectedGroups = new List<UnitGroup>(10);
        }

        public override void Update()
        {
            if (Input.RightMouseWasPressed())
            {
                CreateOrAssignUnitGroup();
            }

            UpdateUnitGroups();
        }

        private void UpdateUnitGroups()
        {
            
            for (int i = UnitGroups.Count - 1; i >= 0; i--)
            {
                // Cull empty or stale unit groups
                if (UnitGroups[i].IsStale())
                {
                    _unitGroupPool.Release(UnitGroups[i]);
                    UnitGroups.RemoveAt(i);
                    continue;
                }

                UnitGroups[i].Update();
            }
        }

        private void CreateOrAssignUnitGroup()
        {
            List<Entity> units = Scene.GetComponent<UnitSelector>().GetSelectedUnits();
            Vector2 destination = Scene.Camera.GetWorldLocation(Input.MouseScreenPosition);
            BitArray groupId = GetGroupId(units);
            UnitGroup matchingGroup = GetUnitGroupById(groupId);

            if (matchingGroup != null)
            {
                ReuseUnitGroup(matchingGroup, destination);
            }
            else
            {
                RegisterNewGroup(groupId, units, destination);
            }
        }

        private void RegisterNewGroup(BitArray groupId, List<Entity> units, Vector2 destination)
        {
            UnitGroup group = _unitGroupPool.ObtainItem();
            group.Id = groupId;
            group.Units = units;
            group.Waypoints.Enqueue(destination);
            group.SetStateToMoving();
            group.AssignNewLeader();
            StealUnits(group);

            UnitGroups.Add(group);

            // really only for debug
            group._scene = Scene;
        }

        private void ReuseUnitGroup(UnitGroup group, Vector2 destination)
        {
            if (Input.RightShiftClickOccured())
            {
                group.Waypoints.Enqueue(destination);
            }
            else
            {
                group.Waypoints.Clear();
                group.Waypoints.Enqueue(destination);
                group.CurrentWaypoint = null;
            }
        }

        private void StealUnits(UnitGroup group)
        {
            // A rather nieve implementation
            for (int i = 0; i < group.Units.Count; i ++)
            {
                for (int j = 0; j < UnitGroups.Count; j++)
                {
                    if (UnitGroups[j].Units.Contains(group.Units[i]))
                    {
                        UnitGroups[j].Units.Remove(group.Units[i]);
                        AffectedGroups.Add(UnitGroups[j]);
                        break;
                    }
                }
            }

            UpdateAffectedGroups();
        }

        private void UpdateAffectedGroups()
        {
            for (int i = 0; i < AffectedGroups.Count; i++)
            {
                AffectedGroups[i].AssignNewLeader();
                AffectedGroups[i].Id = GetGroupId(AffectedGroups[i].Units);
            }

            AffectedGroups.Clear();
        }

        private BitArray GetGroupId(List<Entity> entities)
        {
            BitArray groupId = new BitArray(500, false);
            for (int i = 0; i < entities.Count; i++)
            {
                groupId[entities[i].Id] = true;
            }

            return groupId;
        }

        private UnitGroup GetUnitGroupById(BitArray id)
        {
            for (int i = 0; i < UnitGroups.Count; i++)
            {
                if (id.IsEqual(UnitGroups[i].Id))
                {
                    return UnitGroups[i];
                }
            }

            return null;
        }

        private static int CompareEntites(Entity x, Entity y)
        {
            if (x.Id == y.Id)
            {
                return 0;
            }

            if (x.Id > y.Id)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
    }
}
