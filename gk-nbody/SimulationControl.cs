using System;
using ImGuiNET;
using System.Numerics;

namespace GKApp
{
    public class SimulationControl
    {
        private Simulation _simulation;

        public SimulationControl(Simulation simulation)
        {
            _simulation = simulation;
        }

        private void DrawSimControlContents()
        {
            var buttonType = _simulation.SimulationRunning ? ImGuiDir.Right : ImGuiDir.Down;
            if (ImGui.ArrowButton("simState", buttonType))
            {
                _simulation.SimulationRunning = !_simulation.SimulationRunning;
            }
            ImGui.SameLine();
            if (_simulation.SimulationRunning)
                ImGui.TextColored(new Vector4(58.0f/256.0f, 174.0f/256.0f, 8.0f/256.0f, 1.0f), $"Symulacja uruchomiona");
            else
                ImGui.TextColored(new Vector4(174.0f/256.0f, 0.0f, 2.0f/256.0f, 1.0f), $"Symulacja zatrzymana");
            
            if (ImGui.InputInt("Ziarno", ref _simulation.SeedRef(), 1, 10, ImGuiInputTextFlags.AlwaysInsertMode | ImGuiInputTextFlags.CharsDecimal | ImGuiInputTextFlags.CharsScientific | ImGuiInputTextFlags.CharsNoBlank))
            {
                _simulation.ReloadWithSeed(_simulation.Seed);
            }
            
            if (ImGui.Button("Wygeneruj nowe ziarno"))
            {
                _simulation.Seed = new Random().Next();
                _simulation.ReloadWithSeed(_simulation.Seed);
            }
            ImGui.SameLine();
            if (ImGui.Button("Reset"))
            {
                _simulation.ReloadWithSeed(_simulation.Seed);
            }

            
            ImGui.SliderFloat("Predkosc", ref _simulation.GetSimulationSpeed(), 0.1f, 10.0f);
            ImGui.Text($"Pozycja kamery: {_simulation.CameraPos}");
        }

        private void DrawBodyEditorContents()
        {
            var bodies = _simulation.BodyRef();
            // foreach (var body in bodies)
            // {
            // }            

        }

        public void Render()
        {
            ImGui.ShowDemoWindow();
            
            ImGui.Begin("Symulacja", ImGuiWindowFlags.AlwaysAutoResize);
            DrawSimControlContents();
            ImGui.End();

            ImGui.Begin("Edytor ciał", ImGuiWindowFlags.AlwaysAutoResize);
            DrawBodyEditorContents();
            ImGui.End();
        }
    }
}