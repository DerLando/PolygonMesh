using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.TopologyHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyOperations
{
    static class CollapseEdgeOperation
    {
        internal static void CollapseEdge(this Kernel kernel, HalfEdge edge)
        {
            if (EdgeLinker.IsDummyPairEdge(edge))
                edge = edge.Pair;

            // if the edge is naked, we don't have to change the other face
            if (EdgeLinker.IsDummyPairEdge(edge.Pair))
            {
                CollapseNakedEdge(edge, kernel);
                return;
            }

            CollapseEdgeBetweenFaces(edge, kernel);
        }

        private static void CollapseNakedEdge(HalfEdge edge, Kernel kernel)
        {
            kernel.Remove(edge);
            kernel.RemoveUnusedVertices();
        }

        private static void CollapseEdgeBetweenFaces(HalfEdge edge, Kernel kernel)
        {
            kernel.MergeFaces(edge.Face, edge.Pair.Face);
        }
    }
}
