using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Iterators
{
    /// <summary>
    /// An iterator providing access to all Vertices in a <see cref="Face"/>
    /// </summary>
    public class FaceVertexIterator : IEnumerable<Vertex>
    {
        private HalfEdge _start;

        public FaceVertexIterator(Face face)
        {
            _start = face.Start;
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
