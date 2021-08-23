using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GKApp
{
    public class Simulation
    {
        private Body[] _bodies;
        private const float G = 6.6743015151515e-11f;
        private bool _running = false;
        private float _simulationSpeed = 1.0f;

        public Body[] Bodies => _bodies;
        private Dictionary<Keys, bool> _pressedKeys;
        private BodyGenerator _generator;
        private const float _cameraSpeed = 1.0f;
        private float _pitch;
        private float _yaw;
        private int _seed;
        private Vector3 _cameraPos = Vector3.Zero;
        private Vector3 _cameraFront = Vector3.UnitZ;
        private Vector3 _cameraUp = Vector3.UnitY;

        public BodyGenerator Generator => _generator;
        public Vector3 CameraFront => _cameraFront;
        public Vector3 CameraUp => _cameraUp;
        public Vector3 CameraPos { get => _cameraPos; set => _cameraPos = value; }
        public float SimulationSpeed { get => _simulationSpeed; set => _simulationSpeed = value; }
        
        public bool SimulationRunning
        {
            get => _running;
            set => _running = value;
        }

        public int Seed
        {
            get => _seed;
            set => _seed = value;
        }

        public ref int SeedRef()
        {
            return ref _seed;
        }

        public ref Body[] BodyRef()
        {
            return ref _bodies;
        }

        public ref float GetSimulationSpeed()
        {
            return ref _simulationSpeed;
        }

        public Simulation()
        {
            _pressedKeys = new Dictionary<Keys, bool>();
            _seed = new Random().Next();
            _generator = new BodyGenerator();
            Reload();
        }

        public void Reload()
        {
            _bodies = _generator.Generate(_seed);
        }

        public void Update(double delta)
        {
            UpdateKeys();
            if (!_running)
                return;

            delta *= _simulationSpeed;

            for(var mn = 0; mn < _bodies.Length; mn++)
            {
                var b1 = _bodies[mn];
                
                Vector3 F = Vector3.Zero;
                for (var mi = 0; mi < _bodies.Length; mi++)
                {
                    if (mn == mi) continue;
                    var b2 = _bodies[mi];
                    
                    Vector3 subtracted = (b1.Position - b2.Position);
                    Vector3 forces = subtracted.Normalized() / subtracted.LengthSquared;
                    forces *= -(G * b1.Mass * b2.Mass);

                    F += forces;
                }
                
                Vector3 acceleration = (F / b1.Mass);
                _bodies[mn].Velocity += acceleration  * (float)delta;
                _bodies[mn].Position += (b1.Velocity * (float)delta);
                _bodies[mn].Position += (0.5f * acceleration * (float)delta * (float)delta);
            }
        }

        private void UpdateKeys()
        {
            float currentSpeed = _cameraSpeed; 
            if (IsKeyPressed(Keys.LeftShift))
            {
                currentSpeed *= 2.0f;
            }            
            
            if (IsKeyPressed(Keys.W))
            {
                _cameraPos += currentSpeed * _cameraFront;
            }
            if(IsKeyPressed(Keys.S))
            {
                _cameraPos -= currentSpeed * _cameraFront;
            }

            if (IsKeyPressed(Keys.A))
            {
                _cameraPos -= Vector3.Cross(_cameraFront, _cameraUp).Normalized() * currentSpeed;
            }
            
            if (IsKeyPressed(Keys.D))
            {
                _cameraPos += Vector3.Cross(_cameraFront, _cameraUp).Normalized() * currentSpeed;
            } 
        } 
        
        private bool IsKeyPressed(Keys key)
        {
            return _pressedKeys.GetValueOrDefault(key, false);
        }
        
        public void OnKeyDown(KeyboardKeyEventArgs e)
        {
            _pressedKeys[e.Key] = true;
        }
        
        public void OnKeyUp(KeyboardKeyEventArgs e)
        {
            _pressedKeys[e.Key] = false;                
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
