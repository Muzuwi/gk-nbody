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
        public GKProgram() : base(new GameWindowSettings(), new NativeWindowSettings())
        {
            
        }
        
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            if (KeyboardState.IsKeyPressed(Keys.Escape))
            {
                Close();
            }
        }
        
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
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