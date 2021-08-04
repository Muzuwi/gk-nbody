namespace GKApp
{
    public interface ISimulation
    {
        public Body[] Bodies { get; set; }

        public void Update(double delta);
    }
}