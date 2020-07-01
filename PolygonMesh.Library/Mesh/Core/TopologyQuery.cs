using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Core
{
    internal static class TopologyQuery
    {
        internal static bool TryFindMatchingPair(HalfEdge edge, IReadOnlyList<HalfEdge> edges, out HalfEdge pair)
        {
            pair = null;
            if (edge.Pair != null) return false;

            return false;
        }
    }
}
