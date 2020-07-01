using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolygonMesh.Library.Mesh.Elements
{
    /// <summary>
    /// Simple OcTree implementation, will be replaced by something better later
    /// </summary>
    class OcTree<T>
    {
        private List<(Vec3d, T)> _points = new List<(Vec3d, T)>();
        private readonly double _tolerance;

        public OcTree(double tolerance)
        {
            _tolerance = tolerance;
        }

        public bool Insert(T data, Vec3d position)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                if(position.DistanceToSquared(_points[i].Item1) < _tolerance)
                {
                    return false;
                }
            }

            _points.Add((position, data));
            return true;
        }

        public T FindClosest(Vec3d position)
        {
            double minDistance = double.MaxValue;
            int index = -1;

            for (int i = 0; i < _points.Count; i++)
            {
                var distance = position.DistanceToSquared(_points[i].Item1);
                if (distance > minDistance)
                    continue;

                minDistance = distance;
                index = i;
            }

            return _points[index].Item2;
        }

        public IReadOnlyList<T> AsReadOnlyList()
        {
            return (from point in _points select point.Item2).ToList();
        }

        public int Count => _points.Count;
    }
}
