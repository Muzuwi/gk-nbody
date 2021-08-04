using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace GKApp
{
    public class Renderer
    {
        private ISimulation _simulation;
        private ProgramHandle _program;
        
        public Renderer(ISimulation simulation)
        {
            _simulation = simulation;
        }

        public void OnLoad()
        {
            Console.Out.WriteLine("Renderer OnLoad");
            
            string vertexShaderCode = @"
            #version 460 core
            layout (location = 0) in vec3 a_pos;

            out vec3 v_color;
            void main()
            {
                v_color     = vec3(1.0, 0.0, 1.0);
                gl_Position = vec4(a_pos, 0.0); 
            }";

            string fragmentShaderCode = @"
            #version 460 core
            out vec3 frag_color;
            in  vec3 v_color;
      
            void main()
            {
                frag_color = v_color; 
            }";

            var vertexHandle = GL.CreateShader(ShaderType.VertexShaderArb);
            var fragmentHandle = GL.CreateShader(ShaderType.FragmentShaderArb);
            GL.ShaderSource(vertexHandle, vertexShaderCode);
            GL.ShaderSource(fragmentHandle, fragmentShaderCode);

            GL.CompileShader(vertexHandle);
            GL.GetShaderInfoLog(vertexHandle, out string vertexLog);
            if (vertexLog.Trim().Length != 0)
            {
                Console.WriteLine("Failed compiling vertex shader");
                Console.WriteLine(vertexLog);
                return;
            }
            
            GL.CompileShader(fragmentHandle);
            GL.GetShaderInfoLog(fragmentHandle, out string fragmentLog);
            if (fragmentLog.Trim().Length != 0)
            {
                Console.WriteLine("Failed compiling fragment shader");
                Console.WriteLine(fragmentLog);
                return;
            }

            _program = GL.CreateProgram();
            GL.AttachShader(_program, vertexHandle);
            GL.AttachShader(_program, fragmentHandle);
            GL.LinkProgram(_program);
            GL.GetProgramInfoLog(_program, out string programLog);
            if (programLog.Trim().Length != 0)
            {
                Console.WriteLine("Failed linking program");
                Console.WriteLine(programLog);
                return;
            }
        }
        
        public void Render()
        {
            GL.UseProgram(_program);
            
        }
    }
}