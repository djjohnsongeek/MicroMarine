using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zand.ECS.Components;
using Microsoft.Xna.Framework.Graphics;

namespace Zand.ECS.Collections
{
    public class SceneComponentList
    {
        private List<SceneComponent> _components;
        public Scene Scene;

        public SceneComponentList(Scene scene)
        {
            _components = new List<SceneComponent>();
            Scene = scene;
        }

        public void Update()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Update();
            }
        }

        public void Draw()
        {
            Scene.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.NonPremultiplied,
                null, null, null, null, null
            );

            Scene.ShapeBatch.Begin(Scene.Camera.GetTransformation());

            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Draw(Scene.SpriteBatch, Scene.ShapeBatch);
            }


            Scene.SpriteBatch.End();
            Scene.ShapeBatch.End();
        }

        public SceneComponent AddComponent(SceneComponent component)
        {
            _components.Add(component);
            return component;
        }

        public T GetSceneComponent<T>() where T : SceneComponent
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T)
                {
                    return _components[i] as T;
                }
            }
            return null;
        }  
    }
}
