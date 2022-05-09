﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;
using MicroMarine.Components.UnitGroups;
using Zand.Utils;
using System.Collections;

namespace MicroMarine.Components
{
    public class UnitGroupManager : SceneComponent
    {
        private List<UnitGroup> UnitGroups;
        private HashSet<BitArray> GroupIds;
        private List<UnitGroup> AffectedGroups;
        private Pool<UnitGroup> _unitGroupPool;

        public UnitGroupManager(Scene scene) : base(scene)
        {
            _unitGroupPool = new Pool<UnitGroup>(100);
            UnitGroups = new List<UnitGroup>(10);
            GroupIds = new HashSet<BitArray>(10);
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
                    GroupIds.Remove(UnitGroups[i].Id);
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

            if (GroupIds.Contains(groupId))
            {
                ReuseUnitGroup(groupId, destination);
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
            GroupIds.Add(group.Id);
            // really only for debug
            group._scene = Scene;
        }

        private void ReuseUnitGroup(BitArray groupId, Vector2 destination)
        {
            UnitGroup group = GetUnitGroupById(groupId);
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
                GroupIds.Remove(AffectedGroups[i].Id);
                AffectedGroups[i].Id = GetGroupId(AffectedGroups[i].Units);
                GroupIds.Add(AffectedGroups[i].Id);
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
                if (UnitGroups[i].Id == id)
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
