using System;
using System.Linq;
using System.Numerics;
using OpenTK.Mathematics;

namespace GKApp
{
    public class Simulation : ISimulation
    {
        private const double G = 6.6743015151515e-11;

        public Simulation()
        {
            var random = new Random();
            var count = (int)( 25 + random.NextDouble() * 500);
            Bodies = new Body[count];

            foreach(var i in Enumerable.Range(0, count))
            {
                var position = new Vector3d(random.NextDouble(), random.NextDouble(), random.NextDouble());
                var mass = 10 + random.NextDouble() * 1000;
                
                Bodies[i] = new Body(position, new Vector3d(), new Vector3d(), mass);
            }
        }
        
        public Body[] Bodies { get; set; }

        public void Update(double delta)
        {
            /*
             * SUM(F21);
             * F21 = - ( (G * m1 * m2) / |r21 ^ 2| ) * r`21 
             */

            for(int mn = 0; mn < Bodies.Length; mn++)
            {
                Vector3d F = new(0,0,0);
                for (int mi = 0; mi < Bodies.Length; mi++)
                {
                    if (mn == mi) continue;
                    double distance = (Bodies[mn].Position - Bodies[mi].Position).Length;
                    Vector3d result = ((Bodies[mn].Position - Bodies[mi].Position) / distance);
                    result /= Math.Abs(Math.Pow(distance, 2));
                    result *= -(G * Bodies[mn].Mass * Bodies[mi].Mass);
                    F += result;
                }
                Bodies[mn].Acceleration += (F / Bodies[mn].Mass);
                Bodies[mn].Velocity += Bodies[mn].Acceleration * delta;
                Bodies[mn].Position += (Bodies[mn].Velocity * delta) + (0.5 * Bodies[mn].Acceleration * Math.Pow(delta, 2));
            }
        }
    }
}