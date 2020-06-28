using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Iterators
{
    /// <summary>
    /// An iterator iterating counter-clockwise from the initial half-edge
    /// around its incident face
    /// </summary>
    public class EdgeIterator : IEnumerable<HalfEdge>
    {
        private HalfEdge _start;
        private HalfEdge _current;

        public EdgeIterator(HalfEdge edge)
        {
            _start = edge;
            _current = edge;
        }

        public IEnumerator<HalfEdge> GetEnumerator()
        {
            do
            {
                _current = _current.Next;
                yield return _current.Previous;
            } while (_current != _start);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
