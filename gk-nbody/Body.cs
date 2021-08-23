using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace GKApp
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Body
    {
        public Body(Vector3 position, Vector3 velocity, float mass, Vector3 color)
        {
            Position = position;
            Velocity = velocity;
            Mass = mass;
            Color = color;
        }

        public Vector3 Position;
        public Vector3 Velocity;
        public Vector3 Color;
        public float Mass;
    }
}