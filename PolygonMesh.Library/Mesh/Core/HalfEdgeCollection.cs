using PolygonMesh.Library.Mesh.Elements;
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

            return true;

        }
    }
}
