using OpenTK.Mathematics;
using System;
using System.Linq;

namespace GKApp
{
    public class BodyGenerator
    {
        private int _minQuantity;
        private int _maxQuantity;
        private float _minMass;
        private float _maxMass;
        private float _minPosition;
        private float _maxPosition;

        private static Vector3[] _planetColors = new Vector3[21]
        {
            new Vector3(0.596f, 0.003f, 0.003f),
            new Vector3(0.827f, 0.133f, 0.133f),
            new Vector3(0.960f, 0.039f, 0.039f),
            new Vector3(0.533f, 0.003f, 0.003f),
            new Vector3(0.325f, 0.054f, 0.054f),
            new Vector3(0.219f, 0.058f, 0.058f),
            new Vector3(1.000f, 0.356f, 0.160f),
            new Vector3(1.000f, 0.541f, 0.160f),
            new Vector3(0.964f, 0.741f, 0.474f),
            new Vector3(1.000f, 0.854f, 0.560f),
            new Vector3(1.000f, 0.898f, 0.780f),
            new Vector3(0.992f, 0.913f, 0.827f),
            new Vector3(0.988f, 0.827f, 0.290f),
            new Vector3(0.972f, 0.921f, 0.670f),
            new Vector3(0.996f, 0.996f, 0.843f),
            new Vector3(0.843f, 0.980f, 0.996f),
            new Vector3(0.627f, 0.898f, 0.933f),
            new Vector3(0.768f, 0.882f, 0.992f),
            new Vector3(0.921f, 0.956f, 0.976f),
            new Vector3(0.995f, 0.995f, 0.995f),
            new Vector3(0.035f, 0.572f, 0.925f)
        };

        public int MinQuantity { get => _minQuantity; set => _minQuantity = value; }
        public int MaxQuantity { get => _maxQuantity; set => _maxQuantity = value; }
        public float MinMass { get => _minMass; set => _minMass = value; }
        public float MaxMass { get => _maxMass; set => _maxMass = value; }
        public float MinPosition { get => _minPosition; set => _minPosition = value; }
        public float MaxPosition { get => _maxPosition; set => _maxPosition = value; }

        public BodyGenerator()
        {
            _minQuantity = 500;
            _maxQuantity = 750;
            _minMass = 1e4f;
            _maxMass = 1e10f;
            _minPosition = 0.0f;
            _maxPosition = 100.0f;
        }

        public Body[] Generate(int seed)
        {
            var random = new Random(seed);
            var count = (int)(_minQuantity + random.NextDouble() * (_maxQuantity - _minQuantity));
            var bodies = new Body[count];

            foreach (var i in Enumerable.Range(0, count))
            {
                var position = new Vector3(
                    (float)(_minPosition + random.NextDouble() * (_maxPosition - _minPosition)),
                    (float)(_minPosition + random.NextDouble() * (_maxPosition - _minPosition)),
                    (float)(_minPosition + random.NextDouble() * (_maxPosition - _minPosition))
                    );
                var mass = _minMass + random.NextDouble() * (_maxMass - _minMass);
                var color = _planetColors[(int)Math.Round(random.NextDouble() * (_planetColors.Length - 1))];
                
                bodies[i] = new Body(position, new Vector3(), (float)mass, color);
            }

            return bodies;
        }
    }
}
