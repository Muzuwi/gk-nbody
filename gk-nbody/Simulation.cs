using System;
using System.Linq;
using System.Collections.Generic;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GKApp
{
    public class Simulation
    {
        private const float G = 6.6743015151515e-11f;
        private bool _running = true;
        private float _simulationSpeed = 1.0f;

        public Simulation()
        {
            PressedKeys = new Dictionary<Keys, bool>();
            
            var random = new Random();
            var count = (int)( 25 + random.NextDouble() * 500);
            Bodies = new Body[count];

            foreach(var i in Enumerable.Range(0, count))
            {
                var position = new Vector3((float)random.NextDouble()*100, (float)random.NextDouble()*100, (float)random.NextDouble()*100);
                var mass = 1e5 + random.NextDouble() * 1e18;
                var color = new Vector3((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());

                Bodies[i] = new Body(position, new Vector3(), (float)mass, color);
            }
        }
        
        public Body[] Bodies { get; set; }

        private Dictionary<Keys, bool> PressedKeys;
        private const float Speed = 1.0f;
        private float _pitch;
        private float _yaw;
        private Vector3 _cameraPos = Vector3.Zero;
        private Vector3 _cameraFront = Vector3.UnitZ;
        private Vector3 _cameraUp = Vector3.UnitY;

        public Vector3 CameraPos => _cameraPos;
        public Vector3 CameraFront => _cameraFront;
        public Vector3 CameraUp => _cameraUp;


        public void Update(double delta)
        {
                        
            if (IsKeyPressed(Keys.W))
            {
                _cameraPos += Speed * _cameraFront;
            }
            if(IsKeyPressed(Keys.S))
            {
                _cameraPos -= Speed * _cameraFront;
            }

            if (IsKeyPressed(Keys.A))
            {
                _cameraPos -= Vector3.Cross(_cameraFront, _cameraUp).Normalized() * Speed;
            }
            
            if (IsKeyPressed(Keys.D))
            {
                _cameraPos += Vector3.Cross(_cameraFront, _cameraUp).Normalized() * Speed;
            }

            if (!_running)
                return;

            delta *= _simulationSpeed;
            
            /*
             * SUM(F21);
             * F21 = - ( (G * m1 * m2) / |r21 ^ 2| ) * r`21 
             */

            for(int mn = 0; mn < Bodies.Length; mn++)
            {
                Vector3 F = new(0,0,0);
                for (int mi = 0; mi < Bodies.Length; mi++)
                {
                    if (mn == mi) continue;
                    double distance = (Bodies[mn].Position - Bodies[mi].Position).Length;
                    
                    Vector3 forces = ((Bodies[mn].Position - Bodies[mi].Position) / (float)distance);
                    forces /= (float)Math.Abs(Math.Pow(distance, 2));
                    forces *= -(G * Bodies[mn].Mass * Bodies[mi].Mass);
                    F += forces;
                }
                
                Vector3 acceleration = (F / Bodies[mn].Mass);
                Bodies[mn].Velocity += acceleration * (float)delta;
                Bodies[mn].Position += (Bodies[mn].Velocity * (float)delta) + (0.5f * acceleration * (float)Math.Pow(delta, 2));
            }
        }

        public void SetSimulationRunning(bool running)
        {
            _running = running;
        }

        public bool GetSimulationRunning()
        {
            return _running;
        }

        public ref float GetSimulationSpeed()
        {
            return ref _simulationSpeed;
        }


        public bool IsKeyPressed(Keys key)
        {
            return PressedKeys.GetValueOrDefault(key, false);
        }
        
        public void OnKeyDown(KeyboardKeyEventArgs e)
        {
            PressedKeys[e.Key] = true;
        }
        
        public void OnKeyUp(KeyboardKeyEventArgs e)
        {
            PressedKeys[e.Key] = false;                
        }

        public void OnMouseMove(MouseMoveEventArgs e)
        {
            const float sensitivity = 0.5f;
            
            _yaw += e.DeltaX * sensitivity;
            _pitch += -e.DeltaY * sensitivity;
            _pitch = Math.Clamp(_pitch, -89.0f, 89.0f);
            
            Vector3 newFront = Vector3.Zero;
            newFront.X = (float) (Math.Cos(_yaw * Math.PI / 180.0) * Math.Cos(_pitch * Math.PI / 180.0));
            newFront.Y = (float) (Math.Sin(_pitch * Math.PI / 180.0));
            newFront.Z = (float) ((Math.Sin(_yaw * Math.PI / 180.0)) * Math.Cos(_pitch * Math.PI / 180.0));

            _cameraFront = newFront.Normalized();
        }
    }
}
