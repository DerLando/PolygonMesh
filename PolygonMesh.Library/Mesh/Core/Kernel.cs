using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        public int VertexCount => _vertices.Count;
        public int FaceCount => _faces.Count;
        public int HalfEdgeCount => _halfEdges.Count;
        public IEnumerable<HalfEdge> GetEdges => _halfEdges;

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

            EdgeLinker.LinkOrderedEdgeCollection(ref edges);

            face.Start = edges[0];

            _faces.Add(face);
            _halfEdges.AddRange(edges);
        }

        public static Kernel CreateFromPositions(IReadOnlyList<Vec3d> positions, IReadOnlyList<IReadOnlyList<int>> faces)
        {
            var vertices = (from position in positions select new Vertex { Position = position }).ToArray();
            var newFaces = new Face[faces.Count];
            var edges = new List<HalfEdge>();

            for (int i = 0; i < faces.Count; i++)
            {
                newFaces[i] = new Face();
                var faceEdges = new List<HalfEdge>();

                var indices = faces[i];
                for (int j = 0; j < indices.Count; j++)
                {
                    var index = indices[j];

                    var edge = new HalfEdge
                    {
                        Face = newFaces[i],
                        Origin = vertices[index],
                    };

                    if (vertices[index].Outgoing is null)
                        vertices[index].Outgoing = edge;

                    faceEdges.Add(edge);
                }

                EdgeLinker.LinkOrderedEdgeCollection(ref faceEdges);

                newFaces[i].Start = faceEdges[0];

                edges.AddRange(faceEdges);
            }

            EdgeLinker.LinkEdgePairs(ref edges);

            return new Kernel
            {
                _faces = new FaceCollection { _elements = newFaces.ToList() },
                _halfEdges = new HalfEdgeCollection { _elements = edges },
                _vertices = new VertexCollection { _elements = vertices.ToList() }
            };
        }

        public IEnumerable<Vertex> GetFaceVertices(int faceIndex)
        {
            return FaceVertexIterator.FromFace(_faces.GetElement(faceIndex));
        }
    }
}
