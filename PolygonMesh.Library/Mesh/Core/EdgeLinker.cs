﻿using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Core
{
    public static class EdgeLinker
    {
        public static void LinkEdgePairs(ref List<HalfEdge> edges)
        {
            foreach (var edge in edges)
            {
                if (edge.Pair != null) continue;

                foreach (var otherEdge in edges)
                {
                    if (edge == otherEdge) continue;

                    if (edge.Origin == otherEdge.Next.Origin && edge.Next.Origin == otherEdge.Origin)
                    {
                        edge.Pair = otherEdge;
                        otherEdge.Pair = edge;
                    }
                }
            }

            // only true for closed meshes
            //Debug.Assert(edges.All(e => e.Pair != null));
            //Debug.Assert(edges.All(e => e.Pair.Pair == e));
        }

        /// <summary>
        /// Establishes a circular link for an ordered collection of <see cref="HalfEdge"/>s
        /// </summary>
        /// <param name="edges"></param>
        public static void LinkOrderedEdgeCollection(IReadOnlyList<HalfEdge> edges)
        {
            for (int i = 0; i < edges.Count; i++)
            {
                var prev = i > 0 ? i - 1 : edges.Count - 1;
                var next = (i + 1) % edges.Count;

                edges[i].Previous = edges[prev];
                edges[i].Next = edges[next];
            }
        }

        /// <summary>
        /// Tries to link a given HalfEdge inbetween its previous and next edges
        /// </summary>
        /// <param name="edge">The HalfEdge to link</param>
        /// <returns>true on success, false on failure</returns>
        public static bool TryLinkEdge(HalfEdge edge)
        {
            // We can't link without any information
            if (edge.Previous is null && edge.Next is null) return false;

            HalfEdge previous;
            HalfEdge next;

            // we have next information
            if(edge.Previous is null)
            {
                previous = edge.Next.Previous;
                next = edge.Next;
            }
            else // we have previous information
            {
                previous = edge.Previous;
                next = previous.Next;
            }

            // link other edges to this edge
            previous.Next = edge;
            next.Previous = edge;

            // link this edge to other edges
            edge.Previous = previous;
            edge.Next = next;

            return true;
        }
    }
}
