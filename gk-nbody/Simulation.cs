using System;
using System.Linq;
using System.Numerics;
using OpenTK.Mathematics;

namespace GKApp
{
    public class Simulation : ISimulation
    {
        public Simulation()
        {
            var random = new Random();
            var count = (int)(random.NextDouble() * 500);
            Bodies = new Body[count];

            foreach(var i in Enumerable.Range(0, count))
            {
                var position = new Vector3d(random.NextDouble(), random.NextDouble(), random.NextDouble());
                var mass = random.NextDouble() * 1000;
                
                Bodies[i] = new Body(position, new Vector3d(), mass);
            }
        }
        
        public Body[] Bodies { get; set; }

        public void Update(double delta)
        {
            
        }
    }
}