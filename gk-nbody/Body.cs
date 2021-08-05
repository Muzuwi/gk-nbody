using OpenTK.Mathematics;

namespace GKApp
{
    public class Body
    {
        public Body(Vector3 position, Vector3 velocity, Vector3 acceleration, float mass)
        {
            Position = position;
            Velocity = velocity;
            Acceleration = acceleration;
            Mass = mass;
        }

        public Vector3 Position { get; set;  }
        
        public Vector3 Velocity { get; set; } 

        public Vector3 Acceleration { get; set; } 

        public float Mass { get; set; }
        
    }
}