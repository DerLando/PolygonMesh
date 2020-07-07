using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyHelpers
{
    static class ConnectivityQuery
    {
        /// <summary>
        /// Tries to find the shared edge between two given faces
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="shared">The shared edge, if any, or null</param>
        /// <returns>True on success, false on failure</returns>
        internal static bool TryFindSharedEdge(Face a, Face b, out HalfEdge shared)
        {
            shared = null;

            // shared edges between a face and itself are undefined
            if (a == b) return false;

            // iterate over face edges of a
            foreach (var edge in new EdgeIterator(a.Start))
            {
                // iterate over face edges of b
                foreach (var otherEdge in new EdgeIterator(b.Start))
                {
                    // if the faces share an edge, they will be pairs
                    if (edge.Pair != otherEdge)
                        continue;

                    shared = edge;
                    return true;
                }
            }

            return false;
        }

        internal static Vec3d GetFaceCenter(this Face face)
        {
            return 
                Vec3d
                .Average(new FaceVertexIterator(face)
                .Select(v => v.Position)
                .ToList());
        }
    }
}
