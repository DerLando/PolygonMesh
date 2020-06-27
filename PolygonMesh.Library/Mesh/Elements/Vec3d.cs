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

        /// <summary>
        /// Determines if two <see cref="Vec3d"/> are equal under tolerance
        /// </summary>
        /// <param name="other"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        public bool EpsilonEquals(in Vec3d other, double epsilon)
        {
            return
                Math.Abs(other.X - X) < epsilon &&
                Math.Abs(other.Y - Y) < epsilon &&
                Math.Abs(other.Z - Z) < epsilon;
        }
    }
}
