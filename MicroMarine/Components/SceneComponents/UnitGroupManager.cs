using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;
using Zand.AI;
using Zand.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MicroMarine.Components
{
    public class UnitGroupManager : SceneComponent
    {
        private UnitSelector _unitSelector;
        private List<UnitCommand> _allCommands;
        private Texture2D _waypointTexture;

        public UnitGroupManager(Scene scene) : base(scene)
        {
            _unitSelector = Scene.GetComponent<UnitSelector>();
            _waypointTexture = scene.Content.GetContent<Texture2D>("waypoint");
            _allCommands = new List<UnitCommand>();
        }

        public override void Update()
        {
            if (Input.RightMouseWasPressed())
            {
                AssignCommand();
            }

            CullCommands();

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
            _allCommands.Add(newCommand);
        }

        private void CullCommands()
        {
            for (int i = _allCommands.Count - 1; i >= 0; i--)
            {
                if (_allCommands[i].Status == CommandStatus.Completed)
                {
                    _allCommands.RemoveAt(i);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var origin = new Vector2(8, 8);
            foreach (var command in _allCommands)
            {
                spriteBatch.Draw(
                    _waypointTexture,
                    Scene.Camera.GetScreenLocation(command.Destination.Position),
                    null,
                    Color.White,
                    0,
                    origin,
                    1,
                    SpriteEffects.None,
                    1
                );
            }
        }
    }
}
