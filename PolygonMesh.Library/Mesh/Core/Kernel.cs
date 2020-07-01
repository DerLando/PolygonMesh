using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using PolygonMesh.Library.Mesh.TopologyOperations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace PolygonMesh.Library.Mesh.Core
{
    /// <summary>
    /// The mesh kernel, handles topology operations on a mesh.
    /// Basically a repository with additional functionality
    /// </summary>
    internal class Kernel
    {
        #region private fields

        //private VertexCollection _vertices = new VertexCollection();
        private HalfEdgeCollection _halfEdges = new HalfEdgeCollection();
        private FaceCollection _faces = new FaceCollection();
        private OcTree<Vertex> _vertices = new OcTree<Vertex>(.01);

        #endregion

        #region public properties

        internal int VertexCount => _vertices.Count;
        internal int FaceCount => _faces.Count;
        internal int HalfEdgeCount => _halfEdges.Count;
        internal IReadOnlyList<HalfEdge> Edges => _halfEdges.AsReadOnly();
        internal IReadOnlyList<Vertex> Vertices => _vertices.AsReadOnlyList();
        internal IReadOnlyList<Face> Faces => _faces.AsReadOnly();

        #endregion

        #region public topology methods

        /// <summary>
        /// Creates (of finds an existing) vertex for the given position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vertex GetVertexForPosition(Vec3d position)
        {
            var vertex = new Vertex { Position = position };
            if (_vertices.Insert(vertex, position))
                return vertex;

            return _vertices.FindClosest(position);
        }

        /// <summary>
        /// Inserts a <see cref="HalfEdge"/> instance into the kernel.
        /// For this to work the halfedge needs linking information present on its
        /// <see cref="HalfEdge.Next"/> and <see cref="HalfEdge.Previous" /> properties.
        /// If no linking information is present for the <see cref="HalfEdge.Pair"/>, a new pair edge
        /// will be created and inserted as a naked edge
        /// </summary>
        /// <param name="edge"></param>
        /// <returns>true on success, false on failure</returns>
        public bool InsertEdge(HalfEdge edge)
        {
            // try to link up the edge
            if(!EdgeLinker.TryLinkEdge(edge))
                return false;

            // add edge to inner collection
            _halfEdges.Add(edge);

            // enforce a valid edge pair
            if (edge.Pair == null)
            {
                // try to find an already existing HalfEdge, that would make a good pair


                // create pair edge, it has no linking information and is considered 'naked'
                var pair = new HalfEdge
                {
                    Origin = edge.Next.Origin,
                    Pair = edge
                };

                _halfEdges[HalfEdgeCount - 1].Pair = pair;
                _halfEdges.Add(pair);
            }

            return true;
        }

        /// <summary>
        /// Don't use this
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="pair"></param>
        /// <returns></returns>
        public bool InsertEdge(HalfEdge edge, HalfEdge pair)
        {
            // try to link up the edge
            if (!EdgeLinker.TryLinkEdge(edge))
                return false;

            edge.Pair = pair;
            pair.Pair = edge;

            _halfEdges.Add(edge);
            _halfEdges.Add(pair);

            return true;
        }

        public bool InsertFace(Face face)
        {
            // if no linking information is present, we have to abort
            if (face.Start is null) return false;

            _faces.Add(face);
            return true;
        }

        public void SplitFace(HalfEdge start, HalfEdge end)
        {
            TopologyOperation.SplitFace(start, end, this);
        }

        #endregion

        public void AddFace(IEnumerable<Vec3d> positions)
        {
            var face = new Face();
            var edges = new List<HalfEdge>();
            var vertices = new List<Vertex>();

            foreach (var position in positions)
            {
                var vertex = GetVertexForPosition(position);
                var halfEdge = new HalfEdge 
                { 
                    Origin = vertex,
                    Face = face
                };

                if(vertex.Outgoing is null)
                    vertex.Outgoing = halfEdge;

                vertices.Add(vertex);
                edges.Add(halfEdge);
            }

            EdgeLinker.LinkOrderedEdgeCollection(edges);

            face.Start = edges[0];

            //foreach (var vertex in vertices)
            //{
            //    InsertVertex(vertex);
            //}
            InsertFace(face);
            foreach (var edge in edges)
            {
                InsertEdge(edge);
            }
        }

        internal static Kernel CreateFromPositions(IReadOnlyList<Vec3d> positions, IReadOnlyList<IReadOnlyList<int>> faces)
        {
            var kernel = new Kernel();
            var vertices = (from position in positions select kernel.GetVertexForPosition(position)).ToArray();
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

                EdgeLinker.LinkOrderedEdgeCollection(faceEdges);

                newFaces[i].Start = faceEdges[0];

                edges.AddRange(faceEdges);
            }

            EdgeLinker.LinkEdgePairs(ref edges);

            foreach (var face in newFaces)
            {
                kernel.InsertFace(face);
            }

            foreach (var edge in edges)
            {
                kernel.InsertEdge(edge);
            }

            return kernel;
        }

        public IEnumerable<Vertex> GetFaceVertices(int faceIndex)
        {
            return new FaceVertexIterator(_faces[faceIndex]);
        }
    }
}
