using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Iterators
{
    public sealed class VertexIterator : IEnumerable<Vertex>
    {
        private HalfEdge _start;

        public static VertexIterator FromEdge(HalfEdge halfEdge)
        {
            return new VertexIterator { _start = halfEdge };
        }

        public static VertexIterator FromFace(Face face)
        {
            return VertexIterator.FromEdge(face.Start);
        }

        public static VertexIterator FromVertex(Vertex vertex)
        {
            return VertexIterator.FromEdge(vertex.Outgoing);
        }

        public IEnumerator<Vertex> GetEnumerator()
        {
            HalfEdge current = _start;

            do
            {
                current = current.Next;
                yield return current.Origin;
            } while (current != _start);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
