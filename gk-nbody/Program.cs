using System;
using ImGuiOpenTK;
using ImGuiNET;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GKApp
{
    public class GKProgram : GameWindow
    {
        private Simulation _simulation;
        private Renderer _renderer;
        private ImGuiController _imGuiController;
        private SimulationControl _simulationControl;

        public GKProgram() : base(
            new GameWindowSettings(), 
            new NativeWindowSettings()
            {
                Size = new Vector2i(640, 480),
                Title = "N-Body Simulation",
                APIVersion = new System.Version(4, 6),
                API = ContextAPI.OpenGL,
                NumberOfSamples = 8,
            }
            )
        {
            _simulation = new Simulation();
            _simulationControl = new SimulationControl(_simulation);
            _renderer = new Renderer(_simulation);
            _imGuiController = new ImGuiController(640, 480);
        }
        
        protected override void OnLoad()
        {
            base.OnLoad();
            _renderer.OnLoad();
            // CursorGrabbed = true;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            _renderer.OnWindowResize(e.Width, e.Height);
            _imGuiController.WindowResized(e.Width, e.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyPressed(Keys.Escape))
            {
                Close();
            }
            
            _simulation.Update(e.Time);
            _imGuiController.Update(this, (float)e.Time);
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.ClearColor(0.1f, 0.1f, 0.1f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _renderer.Render();
            _simulationControl.Render();
            _imGuiController.Render();
            SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            _simulation.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            _simulation.OnKeyUp(e);
        }

        private bool _holdingLeftMouse = false;
        
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButton.Left)
            {
                _holdingLeftMouse = true;
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButton.Left)
            {
                _holdingLeftMouse = false;
            }
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            if (_holdingLeftMouse && !ImGui.GetIO().WantCaptureMouse)
            {
                _simulation.OnMouseMove(e);    
            }
        }

        public static void Main()
        {
            using (GKProgram sim = new GKProgram())
            {
                sim.Run();
            }
        }
    }
    
}