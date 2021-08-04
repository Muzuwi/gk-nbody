using System;
using System.Linq;
using System.Numerics;
using OpenTK.Mathematics;

namespace GKApp
{
    public class Simulation : ISimulation
    {
        Simulation()
        {
            var random = new Random();
            var count = random.Next();

            foreach(var c in Enumerable.Range(0, count))
            {
                var position = new Vector3d(random.NextDouble(), random.NextDouble(), random.NextDouble());
                var mass = random.NextDouble() * 1000;
                // Bodies.Append()
            }
        }
        
        public Body[] Bodies { get; set; }

        public void Update(double delta)
        {
            throw new System.NotImplementedException();
        }
    }
}