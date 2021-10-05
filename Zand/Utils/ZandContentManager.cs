using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

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
        public Texture2D LoadTexture(string name)
        {
            // let monogame handle stuff compressed by pipline
            if (string.IsNullOrEmpty(Path.GetExtension(name)))
            {
                return LoadTexture(name);
            }

            // check to see if this asset is already loaded
            if (_loadedAssets.TryGetValue(name, out var asset))
            {
                if (asset is Texture2D tex)
                {
                    return tex;
                }
            }

            using (FileStream stream = File.OpenRead(name))
            {
                Texture2D texture = Texture2D.FromStream(Core._instance.GraphicsDevice, stream);
                texture.Name = name;
                _loadedAssets[name] = texture;
                _disposableAssets.Add(texture);

                return texture;
            }
            
        }

        // load sound effects

        #endregion
    }
}