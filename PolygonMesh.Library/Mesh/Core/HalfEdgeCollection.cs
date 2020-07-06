using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using PolygonMesh.Library.Mesh.TopologyHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Core
{
    class HalfEdgeCollection : ElementsCollection<HalfEdge>
    {
        public override bool Insert(HalfEdge edge)
        {
            // try to link up the edge
            if (!EdgeLinker.TryLinkEdge(edge))
                return false;

            Add(edge);

            return true;

        }

        /// <summary>
        /// Add a <see cref="HalfEdge"/> without any linking checks
        /// </summary>
        /// <param name="edge"></param>
        public void Add(HalfEdge edge)
        {
            // add edge to inner collection
            _elements.Add(edge);

            // enforce a valid edge pair
            if (edge.Pair == null)
            {
                // try to find an already existing HalfEdge, that would make a good pair


                // create pair edge, it has no linking information and is considered 'naked'
                var pair = new HalfEdge
                {
                    Origin = edge.Next.Origin,
                    Pair = edge
                };

                _elements[Count - 1].Pair = pair;
                _elements.Add(pair);
            }
        }

        /// <summary>
        /// Removes a <see cref="HalfEdge"/>. This also removes its pair,
        /// as HalfEdges are not allowed to be single ;)
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        public override bool Remove(HalfEdge edge)
        {
            // handle active references to edge
            if (!EdgeLinker.IsDummyPairEdge(edge))
            {
                RemoveReferences(edge);
            }

            // handle active references to its pair
            if (!EdgeLinker.IsDummyPairEdge(edge.Pair))
            {
                RemoveReferences(edge.Pair);
            }

            // unlink the edges
            EdgeLinker.UnlinkEdge(edge);
            EdgeLinker.UnlinkEdge(edge.Pair);

            // remove edges from inner collection
            _elements.Remove(edge);
            _elements.Remove(edge.Pair);

            return true;
        }

        /// <summary>
        /// Removes all references of faces or vertices to the given edge
        /// </summary>
        /// <param name="edge"></param>
        private void RemoveReferences(HalfEdge edge)
        {
            // make sure origin does not point to this
            if (edge.Origin.Outgoing == edge)
            {
                VertexLinker.TryShiftOutgoing(edge.Origin);
            }

            // make sure face does not point to this
            if (edge.Face.Start == edge)
                FaceLinker.TryShiftStart(edge.Face);
        }

    }
}
