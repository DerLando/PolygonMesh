using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Iterators
{
    public class VertexEdgeRingIterator : IEnumerable<HalfEdge>
    {
        private readonly HalfEdge _start;
        private HalfEdge _current;

        public VertexEdgeRingIterator(Vertex vertex)
        {
            _start = vertex.Outgoing;
            _current = _start;
        }

        public IEnumerable<HalfEdge> GetOutgoingEnumerator()
        {
            do
            {
                var edge = _current;
                _current = _current.Pair.Next;
                yield return edge;
            } while (_current != _start);
        }

        public IEnumerator<HalfEdge> GetIncomingEnumerator()
        {
            do
            {
                var edge = _current.Pair;
                _current = _current.Pair.Next;
                yield return edge;
            } while (_current != _start);
        }

        public IEnumerator<HalfEdge> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
