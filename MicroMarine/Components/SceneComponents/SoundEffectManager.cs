using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Zand;
using Zand.AI;
using Zand.Assets;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    class SoundEffectManager : SceneComponent
    {

        private Dictionary<string, SoundEffectInstance> _soundFxBank;

        public SoundEffectManager(Scene scene) : base(scene)
        {
            _soundFxBank = new Dictionary<string, SoundEffectInstance>();
        }

        public void AddSoundEffect(string key, SoundEffect soundEffect)
        {
            _soundFxBank[key] = soundEffect.CreateInstance();
        }

        public void PlaySoundEffect(string name)
        {
            if (name is null)
            {
                return;
            }

            if (_soundFxBank.TryGetValue(name, out SoundEffectInstance sfx))
            {
                if (sfx.State != SoundState.Playing)
                {
                    sfx.IsLooped = true;
                    sfx.Play();
                }
            }
        }

        public void StopSoundEffect(string name)
        {
            if (name is null)
            {
                return;
            }

            if (_soundFxBank.TryGetValue(name, out SoundEffectInstance sfx))
            {
                sfx.Stop();
                sfx.IsLooped = false;
            }
        }

        public void OnAttackingStateChange(object src, StateEventArgs args)
        {
            if (args.EventType == StateEventType.Enter)
            {
                PlaySoundEffect("marineAttack");
            }
            else if (args.EventType == StateEventType.Exit)
            {
                StopSoundEffect("marineAttack");
            }

        }
    }
}
