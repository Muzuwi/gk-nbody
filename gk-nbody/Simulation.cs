using System;
using System.Linq;
using OpenTK.Mathematics;

namespace GKApp
{
    public class Simulation : ISimulation
    {
        private const float G = 6.6743015151515e-11f;

        public Simulation()
        {
            var random = new Random();
            var count = (int)( 25 + random.NextDouble() * 500);
            Bodies = new Body[count];

            foreach(var i in Enumerable.Range(0, count))
            {
                var position = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
                var mass = 10 + random.NextDouble() * 1000;
                
                Bodies[i] = new Body(position, new Vector3(), new Vector3(), (float)mass);
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
                Vector3 F = new(0,0,0);
                for (int mi = 0; mi < Bodies.Length; mi++)
                {
                    if (mn == mi) continue;
                    double distance = (Bodies[mn].Position - Bodies[mi].Position).Length;
                    Vector3 result = ((Bodies[mn].Position - Bodies[mi].Position) / (float)distance);
                    result /= (float)Math.Abs(Math.Pow(distance, 2));
                    result *= -(G * Bodies[mn].Mass * Bodies[mi].Mass);
                    F += result;
                }
                Bodies[mn].Acceleration += (F / Bodies[mn].Mass);
                Bodies[mn].Velocity += Bodies[mn].Acceleration * (float)delta;
                Bodies[mn].Position += (Bodies[mn].Velocity * (float)delta) + (0.5f * Bodies[mn].Acceleration * (float)Math.Pow(delta, 2));
            }
        }
    }
}