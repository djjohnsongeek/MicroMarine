using MicroMarine.Components.Units;
using Microsoft.Xna.Framework;
using Zand;

namespace MicroMarine.Components
{
    class UnitSpawner<T> : Component, Zand.IUpdateable where T : Unit, new()
    {
        public Vector2 Position { get; private set; }
        private int _waveCount;
        private int _waveDelay;
        private double _waveTimer;

        public UnitSpawner(Vector2 position, int waveCount, int waveDelay)
        {
            _waveCount = waveCount;
            _waveDelay = waveDelay;
            _waveTimer = 0;
            Position = position;
        }

        public void Update()
        {
            _waveTimer += Time.DeltaTime;
            if (_waveTimer >= _waveDelay)
            {
                SpawnWave();
                _waveTimer = 0;
            }
        }

        public void SpawnWave()
        {
            for (int i = 0; i < _waveCount; i++)
            {
                var unitSelector = Entity.Scene.GetComponent<UnitSelector>();
                var blant = Entity.Scene.CreateEntity("marine", Position);

                blant.AddComponent(new Blant(2)); // not correct, should be T
                unitSelector.AddUnit(blant);
            }
        }

        public override void OnAddedToEntity()
        {

        }

        public override void OnRemovedFromEntity()
        {
        }
    }
}
