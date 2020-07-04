using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyHelpers
{
    static class FaceLinker
    {
        internal static bool TryShiftStart(Face face)
        {
            var iter = new EdgeIterator(face.Start).GetEnumerator();
            iter.MoveNext();
            iter.MoveNext();
            face.Start = iter.Current;

            return true;
        }
    }
}
