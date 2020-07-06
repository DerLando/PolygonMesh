using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Mesh.TopologyHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyOperations
{
    static class RemoveUnusedEdgesOperation
    {
        /// <summary>
        /// Removes all unused Edges from the kernel
        /// Unused edges are edge-pairs, where both pairs are naked
        /// </summary>
        /// <param name="kernel"></param>
        internal static void RemoveUnusedEdges(this Kernel kernel)
        {
            foreach (var edge in kernel.Edges)
            {
                if (!EdgeLinker.IsDummyPairEdge(edge))
                    continue;

                if (!EdgeLinker.IsDummyPairEdge(edge.Pair))
                    continue;

                kernel.Remove(edge);
            }
        }
    }
}
