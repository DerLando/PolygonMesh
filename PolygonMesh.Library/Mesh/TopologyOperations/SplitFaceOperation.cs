using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyOperations
{
    internal static class SplitFaceOperation
    {
        internal static void Split(HalfEdge start, HalfEdge end, Kernel kernel)
        {
            // TODO: Error checks

            // get edges for face of both halfedges
            var edges = new EdgeIterator(start).ToArray();

            // get the index of the end edge inside the edges array
            var endIndex = Array.IndexOf(edges, end);

            // create the new ending halfedge for the start half
            var newEnd = new HalfEdge
            {
                Face = start.Face,
                Next = start,
                Previous = edges[endIndex - 1],
                Origin = end.Origin
            };

            // create the new Face for the end half
            var newFace = new Face { Start = end };
            // create a new starting edge for the end half
            var newStart = new HalfEdge
            {
                Face = newFace,
                Next = end,
                Previous = edges.Last(),
                Origin = end.Origin,
                Pair = newEnd
            };

            // pair up the new end
            newEnd.Pair = newStart;

            // assign second half of edges array to new face
            for (int i = endIndex; i < edges.Length; i++)
            {
                edges[i].Face = newFace;
            }

            // add new edges
            kernel.InsertEdge(newEnd);
            kernel.InsertEdge(newStart);

            // add new face
            kernel.InsertFace(newFace);
        }
    }
}
