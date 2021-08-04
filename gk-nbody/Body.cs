using OpenTK.Mathematics;

namespace GKApp
{
    public class Body
    {
        public Body(Vector3d position, Vector3d velocity, double mass)
        {
            Position = position;
            Velocity = velocity;
            Mass = mass;
        }

        public Vector3d Position { get; set;  }
        
        public Vector3d Velocity { get; set; } 

        public double Mass { get; set; }
        
    }
}