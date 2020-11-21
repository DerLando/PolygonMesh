using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyQueries
{
    interface IElementsQuery<T>
    {
        T Execute();
    }
}
