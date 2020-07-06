using PolygonMesh.Library.Mesh.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyOperations
{
    static class RemoveUnusedVerticesOperation
    {
        /// <summary>
        /// Tests all vertices, to see if they are still used
        /// and removes them if it is not the case
        /// </summary>
        /// <param name="kernel"></param>
        internal static void RemoveUnusedVertices(this Kernel kernel)
        {
            foreach (var vertex in kernel.Vertices)
            {
                if (vertex.Outgoing is null)
                    kernel.Remove(vertex);
            }
        }
    }
}
