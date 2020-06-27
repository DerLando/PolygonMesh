using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Elements
{
    public readonly struct Vec3d
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Vec3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
