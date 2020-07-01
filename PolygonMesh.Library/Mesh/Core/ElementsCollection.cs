using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Core
{
    public abstract class ElementsCollection<T> : IList<T>
    {
        internal List<T> _elements;

        protected ElementsCollection()
        {
            _elements = new List<T>();
        }

        public IReadOnlyList<T> AsReadOnly()
        {
            return _elements;
        }

        public abstract bool Insert(T element);

        public T this[int index] { get => _elements[index]; set => _elements[index] = value; }

        public int Count => _elements.Count;

        public bool IsReadOnly => false;

        public virtual void Add(T element)
        {
            _elements.Add(element);
        }

        public virtual void AddRange(IEnumerable<T> elements)
        {
            _elements.AddRange(elements);
        }

        public void Clear()
        {
            _elements = new List<T>();
        }

        public bool Contains(T item)
        {
            return _elements.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
