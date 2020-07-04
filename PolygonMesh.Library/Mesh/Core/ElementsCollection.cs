using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Core
{
    public abstract class ElementsCollection<T> : IReadOnlyList<T>
    {
        internal List<T> _elements;

        protected ElementsCollection()
        {
            _elements = new List<T>();
        }

        public abstract bool Insert(T element);

        public abstract bool Remove(T element);

        public T this[int index] { get => _elements[index]; set => _elements[index] = value; }

        public int Count => _elements.Count;

        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
