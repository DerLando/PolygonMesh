using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Iterators
{
    public sealed class FaceVertexIterator : IEnumerable<Vertex>
    {
        private HalfEdge _start;

        public static FaceVertexIterator FromEdge(HalfEdge halfEdge)
        {
            return new FaceVertexIterator { _start = halfEdge };
        }

        public static FaceVertexIterator FromFace(Face face)
        {
            return FaceVertexIterator.FromEdge(face.Start);
        }

        public static FaceVertexIterator FromVertex(Vertex vertex)
        {
            return FaceVertexIterator.FromEdge(vertex.Outgoing);
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
