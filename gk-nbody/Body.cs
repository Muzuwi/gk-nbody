using OpenTK.Mathematics;

namespace GKApp
{
    public class Body
    {
        public Body(Vector3 position, Vector3 velocity, float mass, Vector3 color)
        {
            Position = position;
            Velocity = velocity;
            Mass = mass;
            Color = color;
        }

        public Vector3 Position { get; set;  }
        
        public Vector3 Velocity { get; set; }

        public Vector3 Color { get; set; }

        public float Mass { get; set; }
    }
}