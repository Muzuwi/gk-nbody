using System;
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
                _simulation.SimulationRunning = !_simulation.SimulationRunning;
            }
            ImGui.Text($"Ziarno: {_simulation.Seed}");
            if (ImGui.Button("Wygeneruj"))
            {
                _simulation.Seed = new Random().Next();
                _simulation.ReloadWithSeed(_simulation.Seed);
            }
            if (ImGui.Button("Reset"))
            {
                _simulation.ReloadWithSeed(_simulation.Seed);
            }

            ImGui.SliderFloat("Predkosc", ref _simulation.GetSimulationSpeed(), 0.1f, 10.0f);
            ImGui.Text($"Pozycja kamery: {_simulation.CameraPos}");

            ImGui.End();
        }

    }
}