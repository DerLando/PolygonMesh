using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PolygonMesh.Library.Mesh.TopologyHelpers
{
    internal static class VertexLinker
    {
        internal static bool TryShiftOutgoing(Vertex vertex)
        {
            try
            {
                var iter =
                    new VertexEdgeRingIterator(vertex)
                    .GetOutgoingEnumerator()
                    .Where(e => !EdgeLinker.IsDummyPairEdge(e)) // Only shift to a valid edge
                    .ToArray();

                if (iter.Length <= 1)
                {
                    vertex.Outgoing = null;
                    return false;
                }

                vertex.Outgoing = iter[1];
                return true;
            }
            catch (Exception)
            {
                // TODO: evaluate exception, or even better, fix it
                vertex.Outgoing = null;
                return false;
            }

        }
    }
}
