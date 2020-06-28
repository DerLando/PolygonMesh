using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Iterators
{
    /// <summary>
    /// An iterator providing access to the one-ring vertices around a <see cref="Vertex"/>
    /// </summary>
    public class VertexRingIterator : IEnumerable<Vertex>
    {
        private readonly HalfEdge _start;
        private HalfEdge _current;

        public VertexRingIterator(Vertex vertex)
        {
            _start = vertex.Outgoing;
            _current = _start;
        }

        public IEnumerator<Vertex> GetEnumerator()
        {
            do
            {
                var vertex = _current.Target;
                _current = _current.Pair.Next;
                yield return vertex;
            } while (_current != _start);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
