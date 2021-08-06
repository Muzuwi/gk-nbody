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
        private Body[] _bodies;
        private const float G = 6.6743015151515e-11f;
        private bool _running = false;
        private float _simulationSpeed = 1.0f;
        private static Vector3[] _planetColors = new Vector3[18]
        {
            new Vector3(0.596f, 0.003f, 0.003f),
            new Vector3(0.827f, 0.133f, 0.133f),
            new Vector3(0.960f, 0.039f, 0.039f),
            new Vector3(0.533f, 0.003f, 0.003f),
            new Vector3(0.325f, 0.054f, 0.054f),
            new Vector3(0.219f, 0.058f, 0.058f),
            new Vector3(1.000f, 0.356f, 0.160f),
            new Vector3(1.000f, 0.541f, 0.160f),
            new Vector3(0.964f, 0.741f, 0.474f),
            new Vector3(1.000f, 0.854f, 0.560f),
            new Vector3(1.000f, 0.898f, 0.780f),
            new Vector3(0.992f, 0.913f, 0.827f),
            new Vector3(0.988f, 0.827f, 0.290f),
            new Vector3(0.972f, 0.921f, 0.670f),
            new Vector3(0.996f, 0.996f, 0.843f),
            new Vector3(0.843f, 0.980f, 0.996f),
            new Vector3(0.627f, 0.898f, 0.933f),
            new Vector3(0.768f, 0.882f, 0.992f)
        };

        public Body[] Bodies => _bodies;
        private Dictionary<Keys, bool> _pressedKeys;
        private const float _cameraSpeed = 1.0f;
        private float _pitch;
        private float _yaw;
        private int _seed;
        private Vector3 _cameraPos = Vector3.Zero;
        private Vector3 _cameraFront = Vector3.UnitZ;
        private Vector3 _cameraUp = Vector3.UnitY;

        public Vector3 CameraPos => _cameraPos;
        public Vector3 CameraFront => _cameraFront;
        public Vector3 CameraUp => _cameraUp;

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
            ReloadWithSeed(_seed);
        }

        public void ReloadWithSeed(int seed)
        {
            var random = new Random(seed);
            var count = (int)( 250 + random.NextDouble() * 500);
            _bodies = new Body[count];

            foreach(var i in Enumerable.Range(0, count))
            {
                var position = new Vector3((float)random.NextDouble()*100, (float)random.NextDouble()*100, (float)random.NextDouble()*100);
                var mass = 1e2 + random.NextDouble() * 1e10;
                var color = _planetColors[(int)Math.Round(random.NextDouble() * 16)];
                _bodies[i] = new Body(position, new Vector3(), (float)mass, color);
            }
        }

        public void Update(double delta)
        {
            UpdateKeys();
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
