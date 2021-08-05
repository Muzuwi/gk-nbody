using OpenTK.Mathematics;

namespace GKApp
{
    public class Body
    {
        public Body(Vector3 position, Vector3 velocity, float mass)
        {
            Position = position;
            Velocity = velocity;
            Mass = mass;
        }

        public Vector3 Position { get; set;  }
        
        public Vector3 Velocity { get; set; } 

        public float Mass { get; set; }
        
    }
}