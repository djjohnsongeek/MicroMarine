using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    class SoundEffectManager : SceneComponent
    {

        private Dictionary<string, List<SoundEffectInstance>> _soundFxBank;
        private List<TrackedSoundEffect> _nowPlaying;

        public SoundEffectManager(Scene scene) : base(scene)
        {
            _soundFxBank = new Dictionary<string, List<SoundEffectInstance>>();
            _nowPlaying = new List<TrackedSoundEffect>();
        }

        public void AddSoundEffect(string name, SoundEffect soundEffect, int instanceCount, float volume = 1)
        {
            if (!_soundFxBank.ContainsKey(name))
            {
                _soundFxBank[name] = new List<SoundEffectInstance>();
            }

            for (int i = 0; i < instanceCount; i++)
            {
                var sfx = soundEffect.CreateInstance();
                sfx.Volume = volume;
                _soundFxBank[name].Add(sfx);
            }
        }

        public void PlaySoundEffect(string name, Entity entity = null)
        {
            if (_soundFxBank.TryGetValue(name, out List<SoundEffectInstance> sfxInstances))
            {
                int index = Scene.Rng.Next(0, sfxInstances.Count);
                if (sfxInstances[index].State == SoundState.Playing)
                {
                    return;
                }
                sfxInstances[index].Play();

                // new Tracked sound effect will definately cause mem ssies TODO
                _nowPlaying.Add(new TrackedSoundEffect(name, sfxInstances[index], entity));
            }
        }

        public void StopSoundEffect(string name, Entity entity)
        {
            int? index = null;
            for (int i = 0; i < _nowPlaying.Count; i++)
            {
                if (_nowPlaying[i].Entity is null) continue;


                if (_nowPlaying[i].Entity.Id == entity.Id && _nowPlaying[i].Name == name)
                {
                    _nowPlaying[i].SoundEffect.Stop();
                    index = i;
                    break;
                }
            }

            if (index.HasValue)
            {
                _nowPlaying.RemoveAt(index.Value);
            }
        }

        public void StopAllSoundEffects(Entity entity)
        {
            int? index = null;
            for (int i = 0; i < _nowPlaying.Count; i++)
            {
                if (_nowPlaying[i].Entity is null) continue;
                if (_nowPlaying[i].Entity.Id == entity.Id)
                {
                    _nowPlaying[i].SoundEffect.Stop();
                    index = i;
                    break;
                }
            }

            if (index.HasValue)
            {
                _nowPlaying.RemoveAt(index.Value);
            }
        }


        public override void Update()
        {
            for (int i = _nowPlaying.Count - 1; i >= 0; i--)
            {
                if (_nowPlaying[i].SoundEffect.State == SoundState.Stopped)
                {
                    _nowPlaying.RemoveAt(i);
                }
            }
        }
        // list of sound effects currenly playing
        // on update run through and remove those that are done
        // or if the entity is destroyed
    }

    public class TrackedSoundEffect
    {
        public SoundEffectInstance SoundEffect;
        public Entity Entity;
        public string Name;

        public TrackedSoundEffect(string name, SoundEffectInstance sfx, Entity entity)
        {
            SoundEffect = sfx;
            Entity = entity;
            Name = name;
        }
    }
}
