using System;
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
            _renderer = new Renderer(_simulation);
        }
        
        protected override void OnLoad()
        {
            base.OnLoad();
            _renderer.OnLoad();
            CursorGrabbed = true;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            _renderer.OnWindowResize(e.Width, e.Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyPressed(Keys.Escape))
            {
                Close();
            }
            
            _simulation.Update(e.Time * 10);
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.ClearColor(0.2588235294117647f, 0.2588235294117647f, 0.2588235294117647f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            _renderer.Render();
            
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

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            _simulation.OnMouseMove(e);
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