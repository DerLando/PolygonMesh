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

        public override bool Remove(HalfEdge edge)
        {
            // easy for dummy edges
            if (EdgeLinker.IsDummyPairEdge(edge))
                return _elements.Remove(edge);

            // make sure origin does not point to this
            if(edge.Origin.Outgoing == edge)
            {
                VertexLinker.TryShiftOutgoing(edge.Origin);
            }

            // make sure face does not point to this
            if (edge.Face.Start == edge)
                FaceLinker.TryShiftStart(edge.Face);

            // unlink the edge
            EdgeLinker.UnlinkEdge(edge);

            // remove edge from inner collection
            return _elements.Remove(edge);
        }

    }
}
