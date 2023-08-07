using MicroMarine.Components.Units;
using Microsoft.Xna.Framework;
using Zand;

namespace MicroMarine.Components
{
    class UnitSpawner<T> : Component, Zand.IUpdateable where T : Unit, new()
    {
        public Vector2 Position { get; private set; }
        private int _unitPerWave;
        private int _waveDelay;
        private int _waveStep;
        private double _waveTimer;
        public int _totalSpawns;
        public int _spawnCount;

        public UnitSpawner(Vector2 position, int totalSpawns, int unitPerWave, int waveDelay, int waveStep = 0)
        {
            _totalSpawns = totalSpawns;
            _spawnCount = 0;
            _unitPerWave = unitPerWave;
            _waveDelay = waveDelay;
            _waveTimer = 0;
            _waveStep = waveStep;
            Position = position;
        }

        public void Update()
        {
            if (_spawnCount >= _totalSpawns)
            {
                return;
            }

            _waveTimer += Time.DeltaTime;
            if (_waveTimer >= _waveDelay)
            {
                SpawnWave();

            }
        }

        public void SpawnWave()
        {
            for (int i = 0; i < _unitPerWave; i++)
            {
                Vector2 unitPosition = Calc.RandomPosition(Entity.Scene.Rng, Position, 60);
                var unitSelector = Entity.Scene.GetComponent<UnitSelector>();
                var blant = Entity.Scene.CreateEntity("unit", unitPosition, new Point(16, 16));

                blant.AddComponent(new Scuttle(2)); // not correct, should be T ...
                unitSelector.AddUnit(blant);
                _spawnCount++;
            }
            _unitPerWave += _waveStep;
            _waveTimer = 0;
        }

        public override void OnAddedToEntity()
        {

        }

        public override void OnRemovedFromEntity()
        {
        }
    }
}
