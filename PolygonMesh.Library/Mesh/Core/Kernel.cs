using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Core
{
    public class Kernel
    {
        #region private fields

        private VertexCollection _vertices = new VertexCollection();
        private HalfEdgeCollection _halfEdges = new HalfEdgeCollection();
        private FaceCollection _faces = new FaceCollection();

        #endregion

        #region public properties

        public int FaceCount => _faces.Count;
        public int HalfEdgeCount => _halfEdges.Count;

        #endregion

        public void AddNewFace(IEnumerable<Vec3d> positions)
        {
            var face = new Face();
            var edges = new List<HalfEdge>();
            var vertices = new List<Vertex>();

            foreach (var position in positions)
            {
                var vertex = new Vertex { Position = position };
                var halfEdge = new HalfEdge 
                { 
                    Origin = vertex,
                    Face = face
                };

                vertex.Outgoing = halfEdge;

                edges.Add(halfEdge);
            }

            for (int i = 0; i < edges.Count; i++)
            {
                var prev = i > 0 ? i - 1 : edges.Count - 1;
                var next = (i + 1) % edges.Count;

                edges[i].Previous = edges[prev];
                edges[i].Next = edges[next];
            }

            face.Start = edges[0];

            _faces.Add(face);
            _halfEdges.AddRange(edges);
        }

        public IEnumerable<Vertex> GetFaceVertices(int faceIndex)
        {
            return VertexIterator.FromFace(_faces.GetElement(faceIndex));
        }
    }
}
