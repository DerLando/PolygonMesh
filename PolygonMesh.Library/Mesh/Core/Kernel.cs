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
            return _halfEdges.Insert(edge);
        }

        public bool InsertFace(Face face)
        {
            return _faces.Insert(face);
        }

        public bool InsertFace(IReadOnlyList<HalfEdge> edges)
        {
            return _faces.Insert(edges);
        }

        public void SplitFace(HalfEdge start, HalfEdge end)
        {
            TopologyOperation.SplitFace(start, end, this);
        }

        #endregion

        /// <summary>
        /// Adds a new face to the kernel
        /// </summary>
        /// <param name="positions"></param>
        public void AddFace(IEnumerable<Vec3d> positions)
        {
            var edges = new List<HalfEdge>();

            // iterate over all positions
            foreach (var position in positions)
            {
                // get a vertex instance for the current positions
                var vertex = GetVertexForPosition(position);

                // create a new halfedge originating from the current vertex and linked to the new face
                var halfEdge = new HalfEdge 
                { 
                    Origin = vertex,
                };

                // test if the vertex already has an outgoing edge assigned
                if(vertex.Outgoing is null)
                    vertex.Outgoing = halfEdge;

                edges.Add(halfEdge);
            }

            // Insert a face from the edges
            InsertFace(edges);

            // iterate over all edges and insert them
            foreach (var edge in edges)
            {
                InsertEdge(edge);
            }
        }

        /// <summary>
        /// Create a new kernel from a list of positions and a nested list of indices representing faces
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="faces"></param>
        /// <returns></returns>
        internal static Kernel CreateFromPositions(IReadOnlyList<Vec3d> positions, IReadOnlyList<IReadOnlyList<int>> faces)
        {
            // create new kernel
            var kernel = new Kernel();

            // insert a vertex into the kernel for each position
            var vertices = (from position in positions select kernel.GetVertexForPosition(position)).ToArray();
            var edges = new List<HalfEdge>();

            // iterate over number of faces
            for (int i = 0; i < faces.Count; i++)
            {
                // empty list to hold halfedges of new face
                var faceEdges = new List<HalfEdge>();

                // get vertex indices and iterate over them
                var indices = faces[i];
                for (int j = 0; j < indices.Count; j++)
                {
                    var vertexIndex = indices[j];

                    // create a new halfedge originating from the current vertexindex
                    var edge = new HalfEdge
                    {
                        Origin = vertices[vertexIndex],
                    };

                    // assure outgoing edge on current vertex
                    if (vertices[vertexIndex].Outgoing is null)
                        vertices[vertexIndex].Outgoing = edge;

                    faceEdges.Add(edge);
                }

                // insert a new face from the face edges
                kernel.InsertFace(faceEdges);

                edges.AddRange(faceEdges);
            }

            // link all edges to their pairs
            EdgeLinker.LinkEdgePairs(ref edges);

            // iterate over all edges and add to kernel
            foreach (var edge in edges)
            {
                kernel.InsertEdge(edge);
            }

            // return the kernel
            return kernel;
        }

        public IEnumerable<Vertex> GetFaceVertices(int faceIndex)
        {
            return new FaceVertexIterator(_faces[faceIndex]);
        }
    }
}
