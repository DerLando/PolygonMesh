using PolygonMesh.Library.Extensions;
using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using PolygonMesh.Library.Mesh.TopologyOperations;
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
        public static Mesh CreateFromPositions(IReadOnlyList<Vec3d> positions, IEnumerable<IEnumerable<int>> faces)
        {
            return new Mesh { _kernel = Kernel.CreateFromPositions(positions, faces) };
        }

        /// <summary>
        /// Creates a new <see cref="Mesh"/> instance with a single face comprised of the given positions in order
        /// </summary>
        /// <param name="positions">Positions in 3d space</param>
        /// <returns></returns>
        public static Mesh CreateSingleFace(IEnumerable<Vec3d> positions)
        {
            var mesh = new Mesh();
            mesh.AddFace(positions);
            return mesh;
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
        public bool SplitFace(int faceIndex)
        {
            var start = _kernel.Faces[faceIndex].Start;
            var end = start.Next.Next;
            return _kernel.TrySplitFace(start, end, out _);
        }

        /// <summary>
        /// Splits a face between the origins of the given face-edges.
        /// </summary>
        /// <param name="faceIndex"></param>
        /// <param name="firstEdgeIndex"></param>
        /// <param name="otherEdgeIndex"></param>
        /// <param name="partsIndices"></param>
        /// <returns></returns>
        public bool SplitFace(int faceIndex, int firstEdgeIndex, int otherEdgeIndex, out (int, int) partsIndices)
        {
            partsIndices = (-1, -1);
            var edges = new EdgeIterator(_kernel.Faces[faceIndex].Start).ToArray();
            var success = _kernel.TrySplitFace(edges[firstEdgeIndex], edges[otherEdgeIndex], out _);

            if (success)
                // Hardcoded!!
                partsIndices = (faceIndex, FaceCount - 1);

            return success;
        }

        /// <summary>
        /// Splits the edge at the given index in two parts.
        /// </summary>
        /// <param name="edgeIndex">Index of edge to split</param>
        /// <param name="t">Normalized parameter along edge length, between 0 and 1</param>
        /// <param name="partIndices">Indices of both parts of the split edge</param>
        /// <returns>true on success, false on failure</returns>
        public bool SplitEdge(int edgeIndex, double t, out (int, int) partIndices)
        {
            partIndices = (-1, -1);

            var success = _kernel.TrySplitEdge(_kernel.Edges[edgeIndex], t, out var parts);

            if (success)
                partIndices = (_kernel.Edges.IndexOf(parts.Item1), _kernel.Edges.IndexOf(parts.Item2));
            return success;
        }

        /// <summary>
        /// Splits the edge at the given face-local index in two parts.
        /// </summary>
        /// <param name="faceIndex">The index of the face, of which the edge should be split</param>
        /// <param name="edgeIndex">The face-local index of the edge to split</param>
        /// <param name="t">Normalized parameter along edge length, between 0 and 1</param>
        /// <param name="partIndices">Face-local indices of both parts of the split edge</param>
        /// <returns>true on success, false on failure</returns>
        public bool SplitEdge(int faceIndex, int edgeIndex, double t, out (int, int) partIndices)
        {
            partIndices = (-1, -1);
            var edges = new EdgeIterator(_kernel.Faces[faceIndex].Start).ToArray();
            var success = _kernel.TrySplitEdge(edges[edgeIndex], t, out var parts);

            if (success)
            {
                edges = new EdgeIterator(_kernel.Faces[faceIndex].Start).ToArray();
                partIndices = (Array.IndexOf(edges, parts.Item1), Array.IndexOf(edges, parts.Item2));
            }

            return success;
        }

        public void CollapseEdge(int edgeIndex)
        {
            _kernel.CollapseEdge(_kernel.Edges[edgeIndex]);
        }

        public void CollapseEdge(int faceIndex, int edgeIndex)
        {
            var edges = new EdgeIterator(_kernel.Faces[faceIndex].Start).ToArray();
            _kernel.CollapseEdge(edges[edgeIndex]);
        }

        public bool MergeFaces(int firstFaceIndex, int otherFaceIndex)
        {
            return _kernel.MergeFaces(_kernel.Faces[firstFaceIndex], _kernel.Faces[otherFaceIndex]);
        }

        public void AddFace(IEnumerable<Vec3d> positions)
        {
            _kernel.AddFace(positions);
        }

        #endregion

    }
}
