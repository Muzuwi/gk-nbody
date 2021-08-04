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

        public GKProgram() : base(new GameWindowSettings(), new NativeWindowSettings())
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