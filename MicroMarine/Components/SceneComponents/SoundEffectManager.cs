using Microsoft.Xna.Framework.Audio;
using Zand;
using Zand.Assets;
using Zand.ECS.Components;

namespace MicroMarine.Components
{
    class SoundEffectManager : SceneComponent
    {
        public SoundEffectManager(Scene scene) : base(scene)
        {

        }

        public void PlaySoundEffect(string name)
        {
            if (name is null)
            {
                return;
            }

            Scene.Content.GetContent<SoundEffect>(name).Play();
        }

        public void OnAnimationStart(object src, AnimatorEventArgs args)
        {
            PlaySoundEffect(DetermineSoundFxName(args.AnimationName));
        }

        public string DetermineSoundFxName(string animationName)
        {
            if (animationName.Contains("Attack"))
            {
                return "marineFire";
            }

            return null;
        }
    }
}
