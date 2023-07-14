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
            _nowPlaying = new List<TrackedSoundEffect>(30);
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

        private float RandomPitch(float min, float max)
        {
            double random = Scene.Rng.NextDouble();
            int negate = Scene.Rng.Next(0, 2);
            if (negate == 1)
            {
                random *= -1;
            }

            return MathUtil.ClampFloat(min, max, (float)random);
        }

        public void PlaySoundEffect(string name, bool limitPlayback = false, bool randomChoice = false, Entity entity = null, bool loop = false)
        {
            // If limiting playback, don't play if identical sound scape
            if (limitPlayback && IsPlaying(name))
            {
                return;
            }

            if (_soundFxBank.TryGetValue(name, out List<SoundEffectInstance> sfxInstances))
            {
                int? index = null;
                if (randomChoice)
                {
                    index = Scene.Rng.Next(0, sfxInstances.Count);
                }
                else
                {
                    for (int i = 0; i < sfxInstances.Count; i++)
                    {
                        if (sfxInstances[i].State != SoundState.Playing)
                        {
                            index = i;
                        }

                    }
                }

                //if (sfxInstances[index].State == SoundState.Playing)
                //{
                //    return;
                //}
                if (index.HasValue)
                {
                    sfxInstances[index.Value].IsLooped = loop;
                    sfxInstances[index.Value].Pitch = RandomPitch(-.18f, .18f);
                    sfxInstances[index.Value].Play();
                    _nowPlaying.Add(new TrackedSoundEffect(name, sfxInstances[index.Value], entity));
                }


                // new Tracked sound effect will definately cause mem ssies TODO

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

        public bool IsPlaying(string name)
        {
            for (int i = 0; i < _nowPlaying.Count; i++)
            {
                if (_nowPlaying[i].Name == name && _nowPlaying[i].SoundEffect.State == SoundState.Playing)
                {
                    return true;
                }
            }

            return false;
        }


        public override void Update()
        {
            for (int i = _nowPlaying.Count - 1; i >= 0; i--)
            {
                if (_nowPlaying[i].SoundEffect.State == SoundState.Stopped || EntityDestroyed(_nowPlaying[i].Entity))
                {
                    _nowPlaying.RemoveAt(i);
                }
            }
        }


        private bool EntityDestroyed(Entity e)
        {
            if (e is null) return false;
            return e.IsDestroyed;
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
