using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using PolygonMesh.Library.Mesh.TopologyHelpers;
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

            // can't split triangles
            if (edges.Length <= 3) return;

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
                Origin = start.Origin,
                Pair = newEnd
            };

            // pair up the new end
            newEnd.Pair = newStart;

            // establish circular link for first face
            var firstFaceEdges = edges.Take(endIndex).ToList();
            firstFaceEdges.Add(newEnd);
            EdgeLinker.LinkOrderedEdgeCollection(firstFaceEdges);

            // hacky re-assignment
            newEnd.Face.Start = newEnd.Next;

            // establish circular link for second face
            var secondFaceEdges = edges.Skip(endIndex).ToList();
            secondFaceEdges.Insert(0, newStart);
            EdgeLinker.LinkOrderedEdgeCollection(secondFaceEdges);
            secondFaceEdges.ForEach(e => e.Face = newFace);

            // Add new edges to kernel
            kernel.Add(newEnd);
            kernel.Add(newStart);

            // add new face
            kernel.Insert(newFace);
        }
    }
}
