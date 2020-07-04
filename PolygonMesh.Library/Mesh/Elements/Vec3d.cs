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

        public override string ToString()
        {
            return $"Vec3d (X: {X}, Y: {Y}, Z: {Z})";
        }

        #region Operator overloading

        public static Vec3d operator +(in Vec3d a) => a;
        public static Vec3d operator -(in Vec3d a) => new Vec3d(-a.X, -a.Y, -a.Z);
        public static Vec3d operator +(in Vec3d a, in Vec3d b) => new Vec3d(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        public static Vec3d operator -(in Vec3d a, in Vec3d b) => a + (-b);
        public static Vec3d operator *(in Vec3d a, double rfs) => new Vec3d(a.X * rfs, a.Y * rfs, a.Z * rfs);

        #endregion

        #region public methods

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

        /// <summary>
        /// Calculates the squared distance between this and another <see cref="Vec3d"/>.
        /// This is a fast way to get a comparable distance metric between two <see cref="Vec3d"/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double DistanceToSquared(in Vec3d other)
        {
            return
                Math.Pow(this.X - other.X, 2) +
                Math.Pow(this.Y - other.Y, 2) +
                Math.Pow(this.Z - other.Z, 2);
        }

        /// <summary>
        /// Calculates a new <see cref="Vec3d"/> between this and another <see cref="Vec3d"/>,
        /// at the normalized parameter t.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vec3d VecAtParameter(in Vec3d other, double t)
        {
            return this + (other - this) * t;
        }

        #endregion

    }
}
