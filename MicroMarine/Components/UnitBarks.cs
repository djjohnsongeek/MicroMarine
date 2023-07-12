using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using Zand;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    public class UnitBarks : SceneComponent
    {
        public List<SoundEffectInstance> ReadyBarks;
        public List<SoundEffectInstance> DeathBarks;
        public List<SoundEffectInstance> ACKBarks;

        public UnitBarks(Scene scene) : base(scene)
        {
            ReadyBarks = new List<SoundEffectInstance>();
            DeathBarks = new List<SoundEffectInstance>();
            ACKBarks = new List<SoundEffectInstance>();
        }

        public void AddBark(SoundEffectInstance bark, BarkType type)
        {
            if (type == BarkType.Ready)
            {
                ReadyBarks.Add(bark);
            }

            if (type == BarkType.ACK)
            {
                ACKBarks.Add(bark);
            }

            if (type == BarkType.Death)
            {
                DeathBarks.Add(bark);
            }
        }

        public void PlayBark(BarkType type)
        {
            if (BarkIsPlaying(type))
            {
                return;
            }

            int i = 0;
            switch (type)
            {
                case BarkType.ACK:
                    i = Scene.Rng.Next(0, ACKBarks.Count);
                    ACKBarks[i].Play();
                    break;
                case BarkType.Death:
                    i = Scene.Rng.Next(0, DeathBarks.Count);
                    DeathBarks[i].Play();
                    break;
                case BarkType.Ready:
                    i = Scene.Rng.Next(0, ReadyBarks.Count);
                    ReadyBarks[i].Play();
                    break;
            }
        }

        private bool BarkIsPlaying(BarkType type)
        {
            bool isPlaying = false;

            if (type == BarkType.ACK || type == BarkType.Ready)
            {
                foreach (var sfx in ACKBarks)
                {
                    if (sfx.State == SoundState.Playing)
                    {
                        isPlaying = true;
                    }
                }

                foreach (var sfx in ReadyBarks)
                {
                    if (sfx.State == SoundState.Playing)
                    {
                        isPlaying = true;
                    }
                }
            }

            if (type == BarkType.Death)
            {
                foreach (var sfx in DeathBarks)
                {
                    if (sfx.State == SoundState.Playing)
                    {
                        isPlaying = true;
                    }
                }
            }

            return isPlaying;
        }
    }

    public enum BarkType
    {
        Ready,
        Death,
        ACK,
    }
}
