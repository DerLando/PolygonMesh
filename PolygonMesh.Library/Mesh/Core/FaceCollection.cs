using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Core
{
    class FaceCollection : ElementsCollection<Face>
    {
        public override bool Insert(Face face)
        {
            // if no linking information is present, we have to abort
            if (face.Start is null) return false;

            _elements.Add(face);
            return true;
        }

        /// <summary>
        /// Insert a new face from a counter-clockwise ordered list of halfedges
        /// </summary>
        /// <param name="edges"></param>
        /// <returns></returns>
        public bool Insert(IReadOnlyList<HalfEdge> edges)
        {
            // TODO: Check if ccw - ordered
            // establish circular link between edges
            EdgeLinker.LinkOrderedEdgeCollection(edges);

            // assign face starting edge
            var face = new Face { Start = edges[0] };

            // assign face to edges
            foreach (var edge in edges)
            {
                edge.Face = face;
            }

            // insert the new face
            return Insert(face);
        }
    }
}
