using System;
using System.Collections;
using System.Collections.Generic;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace GKApp
{
    public class Renderer
    {
        private Simulation _simulation;
        private int _program;
        private int _vertexPositionBuffer;
        private int _vertexArrayHandle;
        private float[] vertexPos;
        private Vector2i _windowSize;

        private readonly string vertexShaderCode = @"
            #version 460 core
            precision highp float;
            layout (location = 0) in vec3 aPos;
            layout (location = 1) in float aMass;
            layout (location = 2) in vec3 aColor;

            uniform mat4 uPMatrix;
            uniform mat4 uVMatrix;
            uniform vec3 uCameraPos;

            out vec3 vColor;
            void main()
            {
                float pointMass = 5e2 + (sqrt(aMass) / 5e4);
                float camDistance = distance(aPos, uCameraPos);
                
                vColor      = aColor;
                gl_Position = uVMatrix * uPMatrix * vec4(aPos, 1.0);
                gl_PointSize = pointMass / distance(aPos, uCameraPos);
            }
        ";

        private readonly string fragmentShaderCode = @"
            #version 460 core
            precision highp float;
            out vec3 FragColor;
            in  vec3 vColor;
      
            void main()
            {
                FragColor = vColor;
            }
        ";

        
        public Renderer(Simulation simulation)
        {
            _simulation = simulation;
        }

        public void OnLoad()
        {
            Console.Out.WriteLine("Renderer OnLoad");
            
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
            return Matrix4.LookAt(_simulation.CameraPos, _simulation.CameraPos + _simulation.CameraFront,
                _simulation.CameraUp);
        }
        
        private Matrix4 CreateProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(
                (float)(45.0 * Math.PI / 180.0), 
                (float)_windowSize.X / (float)_windowSize.Y, 
                0.1f, 
                10000.0f);
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

            vertexPos = new float[_simulation.Bodies.Length * 7];
            int i = 0;
            foreach (Body v in _simulation.Bodies)
            {
                vertexPos[i++] = v.Position.X;
                vertexPos[i++] = v.Position.Y;
                vertexPos[i++] = v.Position.Z;
                vertexPos[i++] = v.Mass;
                vertexPos[i++] = v.Color.X; //R
                vertexPos[i++] = v.Color.Y; //G
                vertexPos[i++] = v.Color.Z; //B
            }

            GL.BufferData(BufferTarget.ArrayBuffer, vertexPos.Length*sizeof(float), vertexPos, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, 7 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, 7 * sizeof(float), 4 * sizeof(float));
            GL.EnableVertexAttribArray(2);

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

            GL.Uniform3(GL.GetUniformLocation(_program, "uCameraPos"), _simulation.CameraPos);

            GL.DrawArrays(PrimitiveType.Points, 0, _simulation.Bodies.Length);


            GL.UseProgram(_program);        
        }
    }
}
