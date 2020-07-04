using PolygonMesh.Library.Mesh.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyOperations
{
    static class RemoveUnusedVerticesOperation
    {
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
