using PolygonMesh.Library.Extensions;
using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using PolygonMesh.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolygonMesh.Library.Mesh.Core
{
    public class Mesh
    {
        private Kernel _kernel = new Kernel();

        #region public properties

        public int VertexCount => _kernel.VertexCount;
        public int HalfEdgeCount => _kernel.HalfEdgeCount;
        public int FaceCount => _kernel.FaceCount;
        public IReadOnlyList<VertexModel> Vertices => GetVertices();
        public IReadOnlyList<EdgeModel> HalfEdges => GetHalfEdges();
        public IReadOnlyList<FaceModel> Faces => GetFaces();

        #region Helper methods

        private VertexModel FromVertex(Vertex vertex)
        {
            return new VertexModel
            {
                Index = _kernel.Vertices.IndexOf(vertex),
                Position = vertex.Position
            };
        }

        private IReadOnlyList<VertexModel> GetVertices()
        {
            var verts = _kernel.Vertices.ToArray();
            var models = new VertexModel[VertexCount];
            for (int i = 0; i < VertexCount; i++)
            {
                models[i] = new VertexModel
                {
                    Index = i,
                    Position = verts[i].Position
                };
            }

            return models;
        }

        private IReadOnlyList<EdgeModel> GetHalfEdges()
        {
            var verts = _kernel.Vertices.ToArray();
            var edges = _kernel.Edges.ToArray();
            var models = new EdgeModel[HalfEdgeCount];

            for (int i = 0; i < HalfEdgeCount; i++)
            {
                models[i] = new EdgeModel
                {
                    Index = i,
                    Origin = new VertexModel
                    {
                        Index = Array.IndexOf(verts, edges[i].Origin),
                        Position = edges[i].Origin.Position
                    },
                    Target = new VertexModel
                    {
                        Index = Array.IndexOf(verts, edges[i].Target),
                        Position = edges[i].Target.Position
                    }
                };
            }

            return models;
        }

        private IReadOnlyList<FaceModel> GetFaces()
        {
            var models = new FaceModel[FaceCount];
            for (int i = 0; i < FaceCount; i++)
            {
                var iterator = new FaceVertexIterator(_kernel.Faces[i]);
                var vertices = from vertex in iterator select FromVertex(vertex);

                models[i] = new FaceModel
                {
                    Index = i,
                    Vertices = vertices.ToArray()
                };
            }

            return models;
        }


        #endregion


        #endregion

        #region static constructors

        /// <summary>
        /// Creates a new <see cref="Mesh"/> instance from 3d positions and index collections that form faces
        /// </summary>
        /// <param name="positions">Positions in 3d space</param>
        /// <param name="faces">collections of indices, matching the faces to be generated from the positions</param>
        /// <returns></returns>
        public static Mesh CreateFromPositions(IReadOnlyList<Vec3d> positions, IReadOnlyList<IReadOnlyList<int>> faces)
        {
            return new Mesh { _kernel = Kernel.CreateFromPositions(positions, faces) };
        }

        /// <summary>
        /// Creates a new <see cref="Mesh"/> instance with a single face comprised of the given positions in order
        /// </summary>
        /// <param name="positions">Positions in 3d space</param>
        /// <returns></returns>
        public static Mesh CreateSingleFace(IReadOnlyList<Vec3d> positions)
        {
            var kernel = new Kernel();
            kernel.AddFace(positions);

            return new Mesh { _kernel = kernel };
        }

        #endregion

        #region public methods

        /// <summary>
        /// Gets all vertices that are connected to the given vertex
        /// </summary>
        /// <param name="vertexIndex">index of the vertex to search neighbors of</param>
        /// <returns></returns>
        public IReadOnlyList<VertexModel> GetVertexNeighbours(int vertexIndex)
        {
            var neighbors = new VertexRingIterator(_kernel.Vertices[vertexIndex]);
            return (from neighbor in neighbors select FromVertex(neighbor)).ToList();
        }

        /// <summary>
        /// Placeholder method
        /// </summary>
        /// <param name="faceIndex"></param>
        public void SplitFace(int faceIndex)
        {
            var start = _kernel.Faces[faceIndex].Start;
            var end = start.Next.Next;
            _kernel.SplitFace(start, end);
        }

        #endregion

    }
}
