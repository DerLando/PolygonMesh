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
        private HalfEdge _current;

        public FaceVertexIterator(Face face)
        {
            _start = face.Start;
            _current = _start;
        }

        public IEnumerator<Vertex> GetEnumerator()
        {
            do
            {
                var vertex = _current.Origin;
                _current = _current.Next;
                yield return vertex;
            } while (_current != _start);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
