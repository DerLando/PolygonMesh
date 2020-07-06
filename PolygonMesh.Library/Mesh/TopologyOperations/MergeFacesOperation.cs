using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using PolygonMesh.Library.Mesh.TopologyHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PolygonMesh.Library.Mesh.TopologyOperations
{
    static class MergeFacesOperation
    {
        /// <summary>
        /// Try to merge the two given faces.
        /// For this to work, the edges belonging to both faces must be able
        /// to form a closed loop.
        /// The first face will be grown and the second face removed on success
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True on success, False on failure</returns>
        internal static bool MergeFaces(this Kernel kernel, Face a, Face b)
        {
            // try to find the shared edge
            if (!ConnectivityQuery.TryFindSharedEdge(a, b, out var shared))
                return false;

            // get edge iterator for face a, starting at shared edge
            var firstEdges = new EdgeIterator(shared).ToArray();

            // get edge iterator for face b starting at shared edge
            var otherEdges = new EdgeIterator(shared.Pair).ToArray();

            // remove the shared edge from the kernel
            kernel.Remove(shared);

            // establish circular link between both face halfes
            EdgeLinker.LinkOrderedEdgeCollection(
                firstEdges
                .Skip(1)
                .Concat(otherEdges.Skip(1))
                .ToList());

            // update face references
            otherEdges.Skip(1).Select(e => e.Face = a);

            // remove face b
            b.Start = null;
            kernel.Remove(b);

            return true;
        }
    }
}
