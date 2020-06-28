using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyOperations
{
    internal static class TopologyOperation
    {
        public static void SplitFace(HalfEdge start, HalfEdge end, Kernel kernel)
        {
            SplitFaceOperation.Split(start, end, kernel);
        }
    }
}
