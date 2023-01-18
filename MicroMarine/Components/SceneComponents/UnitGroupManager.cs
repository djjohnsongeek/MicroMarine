using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;
using Zand.AI;
using Zand.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Zand.Debug;
using Zand.Graphics;

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

            if (!DebugTools.ShowDebug)
            {
                CullCommands();
            }
        }

        private void AssignCommand()
        {
            List<Entity> selectedUnits = _unitSelector.GetSelectedUnits();

            if (selectedUnits.Count == 0) return;

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
                Vector2 position = Scene.Camera.GetScreenLocation(command.Destination.Position);

                // Draw Waypoint
                spriteBatch.Draw(
                    _waypointTexture,
                    position,
                    null,
                    Color.White,
                    0,
                    origin,
                    1,
                    SpriteEffects.None,
                    1
                );


                if (DebugTools.ShowDebug)
                {
                    var circleTexture = Shapes.CreateCircleTexture(command.Destination.Radius * 2);

                    // Draw Destination Radius
                    spriteBatch.Draw(
                        texture: circleTexture,
                        position: position,
                        sourceRectangle: null,
                        color: new Color(200, 100, 100, 100),
                        rotation: 0,
                        origin: new Vector2(circleTexture.Width / 2, circleTexture.Height / 2),
                        scale: 1,
                        effects: SpriteEffects.None,
                        layerDepth: 1
                    );
                }


            }
        }
    }
}
