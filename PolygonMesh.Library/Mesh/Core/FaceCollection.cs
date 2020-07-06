using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using PolygonMesh.Library.Mesh.TopologyHelpers;
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
            if (edges.Count < 3) return false;

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

        /// <summary>
        /// Removes a face from the kernel, by cutting a hole in the mesh
        /// All Edges belonging to this face will become unlinked, naked edges
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public override bool Remove(Face face)
        {
            if(face.Start is null)
            {
                return _elements.Remove(face);
            }

            // iterate over face edges
            foreach (var edge in new EdgeIterator(face.Start))
            {
                // unlink the current edge
                EdgeLinker.UnlinkEdge(edge);
            }

            // remove face from inner collection
            return _elements.Remove(face);
        }
    }
}
