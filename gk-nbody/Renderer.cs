using System;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GKApp
{
    public class Renderer
    {
        private ISimulation _simulation;
        private int _program;
        private int _vertexPositionBuffer;
        private int _vertexArrayHandle;
        private float[] vertexPos;
        private Vector2i _windowSize;

        public Renderer(ISimulation simulation)
        {
            _simulation = simulation;
        }

        public void OnLoad()
        {
            Console.Out.WriteLine("Renderer OnLoad");
            
            string vertexShaderCode = @"
            #version 460 core
            precision highp float;
            layout (location = 0) in vec3 aPos;
            layout (location = 1) in float aMass;

            uniform mat4 uPMatrix;
            uniform mat4 uVMatrix;

            out vec3 vColor;
            void main()
            {
                uPMatrix;
                uVMatrix;
                vColor      = vec3(1.0, 1.0, 0.0);
                gl_Position = uPMatrix * uVMatrix * vec4(aPos, 1.0);
                gl_PointSize = 1.0 + (sqrt(aMass) / 15.0);
            }";

            string fragmentShaderCode = @"
            #version 460 core
            precision highp float;
            out vec3 frag_color;
            in  vec3 vColor;
      
            void main()
            {
                frag_color = vColor; 
            }";

            var vertexHandle = GL.CreateShader(ShaderType.VertexShaderArb);
            var fragmentHandle = GL.CreateShader(ShaderType.FragmentShaderArb);
            GL.ShaderSource(vertexHandle, vertexShaderCode);
            GL.ShaderSource(fragmentHandle, fragmentShaderCode);

            GL.CompileShader(vertexHandle);
            GL.GetShaderInfoLog(vertexHandle, out string vertexLog);
            if (vertexLog.Trim().Length != 0)
            {
                System.Diagnostics.Debug.WriteLine("Failed compiling vertex shader");
                System.Diagnostics.Debug.WriteLine(vertexLog);
                Console.WriteLine("Failed compiling vertex shader");
                Console.WriteLine(vertexLog);
                return;
            }
            
            GL.CompileShader(fragmentHandle);
            GL.GetShaderInfoLog(fragmentHandle, out string fragmentLog);
            if (fragmentLog.Trim().Length != 0)
            {
                System.Diagnostics.Debug.WriteLine("Failed compiling fragment shader");
                System.Diagnostics.Debug.WriteLine(fragmentLog);
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
                System.Diagnostics.Debug.WriteLine("Failed linking program");
                System.Diagnostics.Debug.WriteLine(programLog);
                Console.WriteLine("Failed linking program");
                Console.WriteLine(programLog);
                return;
            }

            GL.CreateBuffers(1, out _vertexPositionBuffer);
            GL.CreateVertexArrays(1, out _vertexArrayHandle);
        }

        private Matrix4 CreateViewMatrix()
        {
            return Matrix4.Identity;
        }
        
        private Matrix4 CreateProjectionMatrix()
        {
            return Matrix4.Identity;
        }

        public void OnWindowResize(int width, int height)
        {
            _windowSize.X = width;
            _windowSize.Y = height;
            GL.Viewport(0, 0, width, height);
        }
        
        public void Render()
        {
            GL.UseProgram(_program);

            GL.BindVertexArray(_vertexArrayHandle);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexPositionBuffer);

            vertexPos = new float[_simulation.Bodies.Length * 4];
            int i = 0;
            foreach(Body v in _simulation.Bodies)
            {
                vertexPos[i++] = v.Position.X;
                vertexPos[i++] = v.Position.Y;
                vertexPos[i++] = v.Position.Z;
                vertexPos[i++] = v.Mass;
            }

            GL.BufferData(BufferTarget.ArrayBuffer, vertexPos.Length*sizeof(float), vertexPos, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, 4 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.Enable(EnableCap.ProgramPointSize);

            GL.GetError();

            var vMatrix = CreateViewMatrix();
            var pMatrix = CreateProjectionMatrix();
            
            GL.UniformMatrix4(GL.GetUniformLocation(_program, "uPMatrix"), false, ref vMatrix);
            var err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                Console.Out.WriteLine($"Error uniform 1 {err.ToString()}");
            }

            GL.UniformMatrix4(GL.GetUniformLocation(_program, "uVMatrix"), false, ref pMatrix);
            err = GL.GetError();
            if (err != ErrorCode.NoError)
            {
                Console.Out.WriteLine($"Error uniform 2 {err.ToString()}");
            }
            
            GL.DrawArrays(PrimitiveType.Points, 0, _simulation.Bodies.Length);
        }
    }
}
