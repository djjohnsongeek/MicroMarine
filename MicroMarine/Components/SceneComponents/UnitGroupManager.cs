using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;
using Zand.AI;
using Zand.Components;

namespace MicroMarine.Components
{
    public class UnitGroupManager : SceneComponent
    {
        private UnitSelector _unitSelector;

        public UnitGroupManager(Scene scene) : base(scene)
        {
            _unitSelector = Scene.GetComponent<UnitSelector>();
        }

        public override void Update()
        {
            if (Input.RightMouseWasPressed())
            {
                AssignCommand();
            }
        }

        private void AssignCommand()
        {
            List<Entity> selectedUnits = _unitSelector.GetSelectedUnits();
            var moveCommand = new UnitCommand(CommandType.Move, null, Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));
            UpdateCommandQueues(selectedUnits, moveCommand);
        }

        private void UpdateCommandQueues(List<Entity> units, UnitCommand newCommand)
        {
            bool isShiftClick = Input.RightShiftClickOccured();
            foreach (var unit in units)
            {
                var unitCommandQueue = unit.GetComponent<CommandQueue>();
                if (!isShiftClick)
                {
                    unitCommandQueue.Clear();
                }
                unitCommandQueue.AddCommand(newCommand);
            }
        }
    }
}
