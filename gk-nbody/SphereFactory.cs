using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKApp
{
    static class SphereFactory
    {
        static public float[] CreateSphere()
        {
            List<float> vertexPosition = new List<float> { };
            var stepElevation = 90 / 6;
            var stepAngle = 360 / 64;
            var radius = 1.0;
            var R = 1;
            for (var elevation = -180; elevation < 180; elevation += stepElevation)
            {
                var radiusXY = R + radius * Math.Cos(elevation * Math.PI / 180);
                var radiusZ = R + radius * Math.Sin(elevation * Math.PI / 180);

                var radiusXY2 = R + radius * Math.Cos((elevation + stepElevation) * Math.PI / 180);
                var radiusZ2 = R + radius * Math.Sin((elevation + stepElevation) * Math.PI / 180);

                for (var angle = 0; angle < 360; angle += stepAngle)
                {

                    var px1 = radiusXY * Math.Cos(angle * Math.PI / 180);
                    var pz1 = radiusZ;
                    var py1 = radiusXY * Math.Sin(angle * Math.PI / 180);

                    var px2 = radiusXY * Math.Cos((angle + stepAngle) * Math.PI / 180);
                    var pz2 = radiusZ;
                    var py2 = radiusXY * Math.Sin((angle + stepAngle) * Math.PI / 180);

                    var px3 = radiusXY2 * Math.Sin(angle * Math.PI / 180);
                    var pz3 = radiusZ2;
                    var py3 = radiusXY2 * Math.Sin(angle * Math.PI / 180);

                    var px4 = radiusXY2 * Math.Cos((angle + stepAngle) * Math.PI / 180);
                    var pz4 = radiusZ2;
                    var py4 = radiusXY2 * Math.Sin((angle + stepAngle) * Math.PI / 180);

                    float[] temp = CreateRectangle((float)px1, (float)py1, (float)pz1, (float)px2, (float)py2, (float)pz2, (float)px3, (float)py3, (float)pz3, (float)px4, (float)py4, (float)pz4);
                    foreach (float v in temp)
                    {
                        vertexPosition.Add(v);
                    }
                }
            }
            return vertexPosition.ToArray();
        }

        static private float[] CreateRectangle(float p1x, float p1y, float p1z, float p2x, float p2y, float p2z, float p3x, float p3y, float p3z, float p4x, float p4y, float p4z)
        {
            float[] vertexPosition = new float[18]
            {
                p1x, p1y, p1z, p2x, p2y, p2z, p4x, p4y, p4z,    //Pierwszy trójkąt
                p1x, p1y, p1z, p4x, p4y, p4z, p3x, p3y, p3z     //Drugi trójkąt
            };

            return vertexPosition;
        }
    }
}
