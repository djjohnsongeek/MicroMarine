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
                if (UnitGroups[i].IsStale())
                {
                    CullUnitGroup(i, UnitGroups[i]);
                }
                else
                {
                    UnitGroups[i].Update();
                }
            }
        }

        private void CullUnitGroup(int index, UnitGroup group)
        {
            _unitGroupPool.Release(group);
            UnitGroups.RemoveAt(index);
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
            UnitGroup newGroup = _unitGroupPool.ObtainItem();
            newGroup.PrepareGroup(groupId, units, destination);

            StealUnits(newGroup);
            UnitGroups.Add(newGroup);

            // really only for debug
            newGroup._scene = Scene;
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
            for (int i = 0; i < group.Units.Count; i ++)
            {
                StealUnit(group.Units[i]);
            }

            UpdateAffectedGroups();
        }

        private void StealUnit(Entity unit)
        {
            for (int i = 0; i < UnitGroups.Count; i++)
            {
                if (UnitGroups[i].Units.Remove(unit))
                {
                    AffectedGroups.Add(UnitGroups[i]);
                    return;
                }
            }
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
            // TODO add a pool for bit arrays too?
            var groupId = new BitArray(Config.UnitGroupIdLength, false);
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
    }
}
