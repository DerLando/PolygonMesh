using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyQueries
{
    internal static class HalfEdgeQuery
    {
        /// <summary>
        /// Tries to find an already existing halfedge for the given vertices
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <param name="result">The matching halfedge, or null</param>
        /// <returns>true on success, false on failure</returns>
        public static bool TryGetHalfEdge(Vertex origin, Vertex target, out HalfEdge result)
        {
            result = null;

            if (origin.Outgoing == null)
                return false;

            // get vertex ring iterator around origin
            var iterator = new VertexEdgeRingIterator(origin);
            var outgoing = iterator.GetOutgoingEnumerator();
            foreach (var edge in iterator.GetOutgoingEnumerator())
            {
                if (edge.Target != target)
                    continue;

                result = edge;
                return true;
            }

            return false;
        }
    }
}
