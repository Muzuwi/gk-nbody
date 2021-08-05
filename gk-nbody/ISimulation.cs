using OpenTK.Mathematics;

namespace GKApp
{
    public interface ISimulation
    {
        public Body[] Bodies { get; set; }

        public void Update(double delta);
        
        public Vector3 CameraPos { get; }
        public Vector3 CameraFront { get; }
        public Vector3 CameraUp { get; }
    }
}