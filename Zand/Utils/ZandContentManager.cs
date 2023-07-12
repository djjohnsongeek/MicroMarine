using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Zand.Utils
{
    public class ZandContentManager : ContentManager
    {
        private Dictionary<string, object> _loadedAssets = new Dictionary<string, object>();
        private List<IDisposable> _disposableAssets = new List<IDisposable>();

        public ZandContentManager (IServiceProvider serviceProvider, string rootDirectory) : base(serviceProvider, rootDirectory)
        {
        }

        #region  Loading Assets
        public Texture2D LoadTexture(string name, string path)
        {
            // let monogame handle compressed assets
            if (string.IsNullOrEmpty(Path.GetExtension(path)))
            {
                return Load<Texture2D>(path);
            }

            if (AssetExists<Texture2D>(name))
            {
                return GetContent<Texture2D>(name);
            }

            using FileStream stream = File.OpenRead(path);
            Texture2D texture = Texture2D.FromStream(Core._instance.GraphicsDevice, stream);
            texture.Name = name;
            _loadedAssets[name] = texture;
            _disposableAssets.Add(texture);
            return texture;
        }

        public SpriteFont LoadFont(string name, string path)
        {
            SpriteFont font = Load<SpriteFont>(path);

            if (AssetExists<SpriteFont>(name))
            {
                return GetContent<SpriteFont>(name);
            }

            _loadedAssets[name] = font;
            return font;
        }

        public SoundEffect LoadSoundEffect(string name, string path)
        {
            
            if (AssetExists<SoundEffect>(name))
            {
                return GetContent<SoundEffect>(name);
            }

            using FileStream stream = File.OpenRead(path);
            SoundEffect sfx = SoundEffect.FromStream(stream);
            sfx.Name = name;
            _loadedAssets[name] = sfx;
            _disposableAssets.Add(sfx);

            return sfx;
        }

        private bool AssetExists<T>(string name)
        {
            if (_loadedAssets.TryGetValue(name, out var asset))
            {
                if (asset is T)
                {
                    return true;
                }
            }

            return false;
        }

        public T GetContent<T>(string contentName)
        {
            return (T)_loadedAssets[contentName];
        }

        // load sound effects

        #endregion
    }
}