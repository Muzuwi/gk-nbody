using ImGuiNET;

namespace GKApp
{
    public class SimulationControl
    {
        private ISimulation _simulation;

        public SimulationControl(ISimulation simulation)
        {
            _simulation = simulation;
        }

        public void Render()
        {
            ImGui.ShowDemoWindow();
        }

    }
}