using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;
using Zand.AI;
using Zand.Components;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Zand.Debug;
using Zand.Graphics;
using Microsoft.Xna.Framework.Input;
using MicroMarine.Components.Units;

namespace MicroMarine.Components
{
    public class UnitGroupManager : SceneComponent
    {
        private List<UnitCommand> _allCommands;
        private Texture2D _waypointTexture;
        private Texture2D _waypointAttackTexture;
        private SoundEffectManager _sfxManager;

        // DO NOT ADD or REMOVE
        private readonly List<Entity> _selectedUnits;

        public UnitGroupManager(Scene scene) : base(scene)
        {
            _selectedUnits = Scene.GetComponent<UnitSelector>().GetSelectedUnits();
            _waypointTexture = scene.Content.GetContent<Texture2D>("waypoint");
            _waypointAttackTexture = scene.Content.GetContent<Texture2D>("waypointAttack");
            _allCommands = new List<UnitCommand>();
            _sfxManager = scene.GetComponent<SoundEffectManager>();
        }

        public override void Update()
        {
            if (Input.RightMouseWasPressed())
            {
                AssignCommand();
            }

            if (!DebugTools.Active)
            {
                CullCommands();
            }

            if (Input.KeyWasReleased(Keys.F))
            {
                for (int i = 0; i < _selectedUnits.Count; i++)
                {
                    var chemLightAbility = _selectedUnits[i].GetComponent<ChemLightAbility>();
                    if (chemLightAbility.OnCoolDown)
                    {
                        continue;
                    }
                    else
                    {
                        chemLightAbility.SpawnChemLight();
                        break;
                    }

                }

            }
        }

        private void AssignCommand()
        {
            if (_selectedUnits.Count == 0) return;


            bool isAttackMove = Input.KeyIsDown(Keys.A);
            UnitCommand command;

            Entity targetEntity = Scene.Physics.GetEntityAtPosition("unit", Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));
            CommandType commandType = isAttackMove ? CommandType.AttackMove : CommandType.Move;

            if (targetEntity != null)
            {
                if (!TargetIsAlly(_selectedUnits, targetEntity))
                {
                    commandType = CommandType.Attack;
                }
            }

            command = new UnitCommand(commandType, targetEntity, Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));

            UpdateCommandQueues(_selectedUnits, command);

            _sfxManager.PlaySoundEffect("mAck", limitPlayback: true, randomChoice: true);
        }

        private UnitAllegiance SelectionAllegiance(List<Entity> units)
        {
            return units[0].GetComponent<UnitAllegiance>();
        }

        private bool TargetIsAlly(List<Entity> selectedUnits, Entity targetEntity)
        {
            var unitAllegiance = SelectionAllegiance(selectedUnits);
            var targetAllegiance = targetEntity.GetComponent<UnitAllegiance>();

            return unitAllegiance.Id == targetAllegiance.Id;
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
            foreach (var command in _allCommands)
            {
                DrawCommand(spriteBatch, command);
            }
        }

        private void DrawCommand(SpriteBatch sBatch, UnitCommand command)
        {
            Vector2 screenPos = Scene.Camera.GetScreenLocation(command.Destination.Position);
            DrawWaypoint(sBatch, command, screenPos);

            if (DebugTools.Active)
            {
                DrawDestinationCircles(sBatch, command, screenPos);
            }
        }

        private bool UnitsAreSelected => _selectedUnits.Count > 0;

        private void DrawWaypoint(SpriteBatch sBatch, UnitCommand command, Vector2 commandScreenPos)
        {
            Texture2D texture;
            if (command.Type == CommandType.Move)
            {
                texture = _waypointTexture;
            }
            else if (command.Type == CommandType.AttackMove)
            {
                texture = _waypointAttackTexture;
            }
            else
            {
                return;
            }

            sBatch.Draw(
                texture,
                commandScreenPos,
                null,
                Color.White,
                0,
                new Vector2(8, 8),
                1,
                SpriteEffects.None,
                1
            );

        }

        private void DrawDestinationCircles(SpriteBatch sBatch, UnitCommand command, Vector2 commandScreenPos)
        {
            var circleTexture = Shapes.CreateCircleTexture(command.Destination.Radius * 2);
            sBatch.Draw(
                texture: circleTexture,
                position: commandScreenPos,
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
