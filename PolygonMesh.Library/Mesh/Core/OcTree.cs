using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolygonMesh.Library.Mesh.Core
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

        /// <summary>
        /// Inserts some data at the given position
        /// </summary>
        /// <param name="data">The data to insert</param>
        /// <param name="position">The position to insert the data at</param>
        /// <returns>True on success, False if already some data present at the given position</returns>
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

        /// <summary>
        /// Finds the data stored closest to the given position
        /// </summary>
        /// <param name="position">The position to query data close to</param>
        /// <returns></returns>
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

        public bool Remove((Vec3d, T) data)
        {
            return _points.Remove(data);
        }
    }
}
