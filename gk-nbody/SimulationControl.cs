using System;
using ImGuiNET;
using System.Numerics;

namespace GKApp
{
    public class SimulationControl
    {
        private Simulation _simulation;
        private int mode = 4;

        private int _quantityMin = 350;
        private int _massMin = 4;
        private float _positionMin = -100f;

        private int _quantityMax = 750;
        private int _massMax = 16;
        private float _positionMax = 100f;


        private string[] md = new string[9]
        {
                "/16",
                "/8",
                "/4",
                "/2",
                "x1",
                "x2",
                "x4",
                "x8",
                "x16"
        };
        private float[] speeds = new float[9]
        {
            0.0625f, 0.125f, 0.25f, 0.5f, 1.0f, 2.0f, 4.0f, 10.0f, 100.0f
        };

        public SimulationControl(Simulation simulation)
        {
            _simulation = simulation;
        }

        private void DrawSimControlContents()
        { 
            if (ImGui.InputInt("Ziarno", ref _simulation.SeedRef(), 1, 10, ImGuiInputTextFlags.AlwaysInsertMode | ImGuiInputTextFlags.CharsDecimal | ImGuiInputTextFlags.CharsScientific | ImGuiInputTextFlags.CharsNoBlank))
            {
                _simulation.Reload();
            }
            
            if (ImGui.Button("Wygeneruj nowe ziarno"))
            {
                _simulation.Seed = new Random().Next();
                _simulation.Reload();
            }

            if(ImGui.DragIntRange2("Elementy", ref _quantityMin, ref _quantityMax, 10.0f, 10, 1000))
            {
                _simulation.Generator.MinQuantity = _quantityMin;
                _simulation.Generator.MaxQuantity = _quantityMax;
            }

            if (ImGui.DragIntRange2("Masa (1e_)", ref _massMin, ref _massMax, 1.0f, 1, 100))
            {
                _simulation.Generator.MinMass = _massMax * 1e10f;
                _simulation.Generator.MaxMass = _massMin * 1e10f;
            }

            if (ImGui.DragFloatRange2("Pozycja", ref _positionMin, ref _positionMax, 10.0f, -1000, 1000))
            {
                _simulation.Generator.MinPosition = _positionMin;
                _simulation.Generator.MaxPosition = _positionMax;
            }

            if (ImGui.Button("Generuj"))
            {
                _simulation.Reload();
            }

            ImGui.Separator();

            var buttonType = _simulation.SimulationRunning ? ImGuiDir.Right : ImGuiDir.Down;
            if (ImGui.ArrowButton("simState", buttonType))
            {
                _simulation.SimulationRunning = !_simulation.SimulationRunning;
            }

            ImGui.SameLine();

            if (_simulation.SimulationRunning)
                ImGui.TextColored(new Vector4(58.0f / 256.0f, 174.0f / 256.0f, 8.0f / 256.0f, 1.0f), $"Symulacja uruchomiona");
            else
                ImGui.TextColored(new Vector4(174.0f / 256.0f, 0.0f, 2.0f / 256.0f, 1.0f), $"Symulacja zatrzymana");

            if (ImGui.SliderInt("Predkosc", ref mode, 0, 8, md[mode]))
            {
                _simulation.SimulationSpeed = speeds[mode];
            }

            ImGui.Separator();

            ImGui.Text($"Pozycja kamery: {_simulation.CameraPos}");

            if (ImGui.Button("Resetuj Pozycje Kamery"))
            {
                _simulation.CameraPos = new OpenTK.Mathematics.Vector3(0.0f, 0.0f, 0.0f);
            }
        }

        public void Render()
        {
            ImGui.SetNextWindowPos(new Vector2(0, 0));
            ImGui.Begin("Symulacja", ImGuiWindowFlags.AlwaysAutoResize);
            DrawSimControlContents();
            ImGui.End();
        }
    }
}