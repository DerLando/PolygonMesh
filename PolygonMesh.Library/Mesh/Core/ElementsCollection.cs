using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Core
{
    public abstract class ElementsCollection<T>
    {
        internal List<T> _elements;

        protected ElementsCollection()
        {
            _elements = new List<T>();
        }

        public int Count => _elements.Count;

        public virtual void Add(T element)
        {
            _elements.Add(element);
        }

        public virtual void AddRange(IEnumerable<T> elements)
        {
            _elements.AddRange(elements);
        }

        public T GetElement(int index)
        {
            return _elements[index];
        }
    }
}
