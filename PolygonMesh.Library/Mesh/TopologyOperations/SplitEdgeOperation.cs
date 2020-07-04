﻿using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.TopologyHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyOperations
{
    internal static class SplitEdgeOperation
    {
        internal static void Split(HalfEdge edge, double t, Kernel kernel)
        {
            // TODO: Error checks
            if (EdgeLinker.IsDummyPairEdge(edge))
                edge = edge.Pair;

            // test valid param
            if (0 > t || t > 1) return;

            // calculate vec at param
            var vec = edge.Origin.Position.VecAtParameter(edge.Target.Position, t);

            // convert to a vertex
            var vertex = kernel.GetVertexForPosition(vec);

            // store edge pair
            var pair = edge.Pair;
            var isDummy = EdgeLinker.IsDummyPairEdge(pair);

            // create new edges
            var firstHalf = new HalfEdge
            {
                Face = edge.Face,
                Previous = edge.Previous,
                Origin = edge.Origin
            };
            var secondHalf = new HalfEdge
            {
                Face = edge.Face,
                Next = edge.Next,
                Origin = vertex
            };
            var pairFirstHalf = new HalfEdge
            {
                Face = pair.Face,
                Previous = pair.Previous,
                Origin = pair.Origin,
                Pair = secondHalf,
            };
            var pairSecondHalf = new HalfEdge
            {
                Face = pair.Face,
                Next = pair.Next,
                Origin = vertex,
                Pair = firstHalf
            };

            // link halves
            firstHalf.Next = secondHalf;
            pairFirstHalf.Next = pairSecondHalf;

            // remove original edges
            kernel.Remove(edge);
            kernel.Remove(pair);

            // add new halves
            if (!isDummy)
            {
                kernel.Insert(firstHalf);
                kernel.Insert(secondHalf);
                kernel.Insert(pairFirstHalf);
                kernel.Insert(pairSecondHalf);
            }
            else
            {
                firstHalf.Pair = null;
                secondHalf.Pair = null;
                kernel.Insert(firstHalf);
                kernel.Insert(secondHalf);
            }
        }
    }
}