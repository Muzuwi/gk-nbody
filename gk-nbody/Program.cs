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
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyPressed(Keys.Escape))
            {
                Close();
            }
            
            _simulation.Update(e.Time);
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            GL.ClearDepth(1.0);
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            _renderer.Render();
            
            SwapBuffers();
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