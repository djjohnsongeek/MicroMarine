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
using Zand.UI;

namespace MicroMarine.Components
{
    public class UnitGroupManager : SceneComponent
    {
        private List<UnitCommand> _allCommands;
        private Texture2D _waypointTexture;
        private Texture2D _waypointAttackTexture;
        private SoundEffectManager _sfxManager;
        private SelectedUnits _selectedUnits;

        public UnitGroupManager(Scene scene) : base(scene)
        {
            _selectedUnits = Scene.GetComponent<SelectedUnits>();
            _waypointTexture = scene.Content.GetContent<Texture2D>("waypoint");
            _waypointAttackTexture = scene.Content.GetContent<Texture2D>("waypointAttack");
            _allCommands = new List<UnitCommand>();
            _sfxManager = scene.GetComponent<SoundEffectManager>();
        }

        public override void Update()
        {
            if (Input.RightMouseWasReleased() && _selectedUnits.UnitsAreSelected && Input.Context == InputContext.UnitControl)
            {
                AssignCommand();
            }

            if (!DebugTools.Active)
            {
                CullCommands();
            }

            if (Input.RightMouseWasReleased() && _selectedUnits.UnitsAreSelected && Input.Context == InputContext.UnitAbilities)
            {
                ActivateLocalAbility<ChemLightAbility>();
            }

            if (Input.KeyWasPressed(Keys.F) && Input.Context == InputContext.UnitControl && _selectedUnits.UnitsAreSelected)
            {
                Input.Context = InputContext.UnitAbilities;
            }
        }

        // Location meaning tha tonly one unit from the group will execute the ability in question.
        private void ActivateLocalAbility<T>() where T : UnitAbility
        {
            bool executed = false;
            for (int i = 0; i < _selectedUnits.Selected.Count; i++)
            {
                var ability = _selectedUnits.Selected[i].GetComponent<T>();
                if (ability.OnCoolDown) continue;
                ability.ExecuteAbility();
                executed = true;
                break;
            }

            if (!executed)
            {
                _sfxManager.PlaySoundEffect("error");
            }

            Input.Context = InputContext.UnitControl;
        }

        private void AssignCommand()
        {
            bool isAttackMove = Input.KeyIsDown(Keys.A);
            UnitCommand command;

            Entity targetEntity = Scene.Physics.GetEntityAtPosition("unit", Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));
            CommandType commandType = isAttackMove ? CommandType.AttackMove : CommandType.Move;

            if (targetEntity != null)
            {
                if (!TargetIsAlly(targetEntity))
                {
                    commandType = CommandType.Attack;
                }
            }

            command = new UnitCommand(commandType, targetEntity, Scene.Camera.GetWorldLocation(Input.MouseScreenPosition));

            UpdateCommandQueues(command);

            _sfxManager.PlaySoundEffect("mAck", limitPlayback: true, randomChoice: true);
        }

        private UnitAllegiance SelectionAllegiance(List<Entity> units)
        {
            return units[0].GetComponent<UnitAllegiance>();
        }

        private bool TargetIsAlly(Entity targetEntity)
        {
            var unitAllegiance = SelectionAllegiance(_selectedUnits.Selected);
            var targetAllegiance = targetEntity.GetComponent<UnitAllegiance>();

            return unitAllegiance.Id == targetAllegiance.Id;
        }

        private void UpdateCommandQueues(UnitCommand newCommand)
        {
            bool isShiftClick = Input.RightShiftClickOccured();
            foreach (var unit in _selectedUnits.Selected)
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
