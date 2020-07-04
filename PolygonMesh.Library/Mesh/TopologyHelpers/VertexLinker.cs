using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyHelpers
{
    internal static class VertexLinker
    {
        internal static bool TryShiftOutgoing(Vertex vertex)
        {
            var iter = new VertexEdgeRingIterator(vertex).GetOutgoingEnumerator();
            iter.MoveNext();
            //iter.MoveNext();
            vertex.Outgoing = iter.Current;

            return true;
        }
    }
}
