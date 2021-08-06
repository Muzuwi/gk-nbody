using ImGuiNET;

namespace GKApp
{
    public class SimulationControl
    {
        private Simulation _simulation;

        public SimulationControl(Simulation simulation)
        {
            _simulation = simulation;
        }
        
        public void Render()
        {
            ImGui.ShowDemoWindow();
            ImGui.Begin("Symulacja");
            if (ImGui.ArrowButton("arr", ImGuiDir.Right))
            {
                _simulation.SetSimulationRunning(!_simulation.GetSimulationRunning());
            }

            ImGui.SliderFloat("Predkosc", ref _simulation.GetSimulationSpeed(), 0.1f, 10.0f);

            ImGui.End();
        }

    }
}