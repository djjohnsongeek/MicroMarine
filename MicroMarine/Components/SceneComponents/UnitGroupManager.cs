using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;
using MicroMarine.Components.UnitGroups;
using Zand.Utils;
using System.Collections;
using MicroMarine.Extensions;
using Zand.AI;

namespace MicroMarine.Components
{
    public class UnitGroupManager : SceneComponent
    {
        private List<UnitGroup> UnitGroups;
        private List<UnitGroup> AffectedGroups;
        private Pool<UnitGroup> _unitGroupPool;
        private UnitSelector _unitSelector;

        public UnitGroupManager(Scene scene) : base(scene)
        {
            _unitGroupPool = new Pool<UnitGroup>(100);
            UnitGroups = new List<UnitGroup>(10);
            AffectedGroups = new List<UnitGroup>(10);
            _unitSelector = Scene.GetComponent<UnitSelector>();
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
            List<Entity> units = _unitSelector.GetSelectedUnits();
            var command = new UnitCommand(CommandType.Move, null, Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));
            BitArray groupId = GetGroupId(units);
            UnitGroup matchingGroup = GetUnitGroupById(groupId);

            if (matchingGroup != null)
            {
                ReuseUnitGroup(matchingGroup, command);
            }
            else
            {
                RegisterNewGroup(groupId, units, command);
            }
        }

        private void RegisterNewGroup(BitArray groupId, List<Entity> units, UnitCommand command)
        {
            UnitGroup newGroup = _unitGroupPool.ObtainItem();
            newGroup.Setup(groupId, units, command);

            StealUnits(newGroup);
            UnitGroups.Add(newGroup);

            // really only for debug
            newGroup._scene = Scene;
        }

        private void ReuseUnitGroup(UnitGroup group, UnitCommand command)
        {
            if (Input.RightShiftClickOccured())
            {
                group.CommandQueue.AddCommand(command);
            }
            else
            {
                group.CommandQueue.Clear();
                group.CommandQueue.AddCommand(command);
            }
        }

        private void StealUnits(UnitGroup newGroup)
        {
            for (int i = 0; i < UnitGroups.Count; i++)
            {
                RemoveUnits(UnitGroups[i], newGroup.Units);
            }

            UpdateAffectedGroups();
        }

        private void RemoveUnits(UnitGroup currentGroup, List<Entity> newGroupUnits)
        {
            for (int i = 0; i < newGroupUnits.Count; i++)
            {
                if (currentGroup.RemoveUnit(newGroupUnits[i]))
                {
                    AffectedGroups.Add(currentGroup);
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
