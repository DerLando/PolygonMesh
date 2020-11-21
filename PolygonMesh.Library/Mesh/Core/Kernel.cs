using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using PolygonMesh.Library.Mesh.TopologyHelpers;
using PolygonMesh.Library.Mesh.TopologyOperations;
using PolygonMesh.Library.Mesh.TopologyQueries;
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

        /// <summary>
        /// The dummy 'outside' face
        /// </summary>
        public static Face Outside = new Face { Start = null };
        
        #endregion

        #region public properties

        internal int VertexCount => _vertices.Count;
        internal int FaceCount => _faces.Count;
        internal int HalfEdgeCount => _halfEdges.Count;
        internal IReadOnlyList<HalfEdge> Edges => _halfEdges;
        internal IReadOnlyList<Vertex> Vertices => _vertices.AsReadOnlyList();
        internal IReadOnlyList<Face> Faces => _faces;

        #endregion

        #region public topology methods

        /// <summary>
        /// Creates (of finds an existing) vertex for the given position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vertex GetVertexForPosition(Vec3d position)
        {
            return VertexQuery.GetVertexForPosition(_vertices, position);
        }

        /// <summary>
        /// Tries to find an already existing halfedge for the given vertices
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <param name="result">The matching halfedge, or null</param>
        /// <returns>true on success, false on failure</returns>
        public bool TryGetHalfEdgeBetweenVertices(Vertex origin, Vertex target, out HalfEdge result)
        {
            return HalfEdgeQuery.TryGetHalfEdge(origin, target, out result);
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
        public bool Insert(HalfEdge edge)
        {
            return _halfEdges.Insert(edge);
        }

        public bool Insert(Face face)
        {
            return _faces.Insert(face);
        }

        /// <summary>
        /// Insert a new face from a counter-clockwise ordered list of halfedges
        /// </summary>
        /// <param name="edges">The edges to insert together with the new face</param>
        /// <returns>true on success, false on failure</returns>
        public bool Insert(IReadOnlyList<HalfEdge> edges)
        {
            return _faces.Insert(edges);
        }

        /// <summary>
        /// Add a new HalfEdge without any linking checks.
        /// A valid pair will be enforced
        /// </summary>
        /// <param name="edge"></param>
        public void Add(HalfEdge edge)
        {
            _halfEdges.Add(edge);
        }

        public bool Remove(Vertex vertex)
        {
            return _vertices.Remove((vertex.Position, vertex));
        }

        public bool Remove(HalfEdge edge)
        {
            return _halfEdges.Remove(edge);
        }

        public bool Remove(Face face)
        {
            return _faces.Remove(face);
        }

        /// <summary>
        /// Removes all unused elements of the kernel
        /// </summary>
        public void CleanUp()
        {
            this.RemoveUnusedVertices();
            this.RemoveUnusedEdges();
        }

        #endregion

        /// <summary>
        /// Adds a new face to the kernel
        /// </summary>
        /// <param name="positions"></param>
        public bool AddFace(IEnumerable<Vec3d> positions)
        {
            // No bad faces please :(
            if (positions.Count() < 3) return false;

            // convert positions to vertices inside the mesh
            var vertices = VertexQuery.GetVerticesForPositions(_vertices, positions);

            // if the positions had stacked vertices, we might not be able to continue
            if (vertices.Count() < 3)
            {
                // iterate over position vertices
                foreach (var vertex in vertices) {
                    // if the vertex was old, it will have an outgoing
                    if (vertex.Outgoing != null) continue;

                    // if it was new we need to remove it to restore state
                    Remove(vertex);
                }

                return false;
            }

            var edges = new List<HalfEdge>();

            // iterate over all positions
            foreach (var vertex in vertices)
            {
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
            Insert(edges);

            // iterate over all edges and insert them
            foreach (var edge in edges)
            {
                // TODO: Maybe we should NOT skip linking checks here
                _halfEdges.Add(edge);
            }

            // test if first face
            if(this.FaceCount == 1)
            {
                var outerRing = new EdgeIterator(
                        _halfEdges[0])
                        .Select(h => h.Pair)
                        .Reverse()
                        .ToList();

                // establish circular link between outside half-edges
                EdgeLinker.LinkOrderedEdgeCollection(outerRing);
            }

            // TODO: Should be method of edgelinker instead
            // Make sure edge pairs either have a starting face or the ghost face
            // Iterators are lazy, calling `Count()` will execute the select body
            _ = edges.Where(e => e.Pair.Face == null).Select(e => e.Pair.Face = Kernel.Outside).Count();

            return true;

            // TODO: Make sure outer halfEdges are linked,too
            // TODO: Right now only inner halfEdges are linked circularly
            // TODO: This leads to crashes on faces with too many naked edges when creating vertexringiterators
            // TODO: For this we will need a 'ghost' face that encompasses all the space outside of the mesh faces
            // TODO: CLosed meshes will have an empty ghost face
        }

        /// <summary>
        /// Create a new kernel from a list of positions and a nested list of indices representing faces
        /// This has no checks, the given input needs to represent a valid mesh to produce a meaningful output
        /// </summary>
        /// <param name="positions"></param>
        /// <param name="faces"></param>
        /// <returns></returns>
        internal static Kernel CreateFromPositionsUnchecked(IReadOnlyList<Vec3d> positions, IEnumerable<IEnumerable<int>> faces)
        {
            // create new kernel
            var kernel = new Kernel();

            // insert a vertex into the kernel for each position
            var vertices = (from position in positions select kernel.GetVertexForPosition(position)).ToArray();
            var edges = new List<HalfEdge>();

            // iterate over number of faces
            foreach (var face in faces)
            {
                // empty list to hold halfedges of new face
                var faceEdges = new List<HalfEdge>();

                // get vertex indices and iterate over them
                var indices = face;
                foreach (var vertexIndex in indices)
                {
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
                kernel.Insert(faceEdges);

                edges.AddRange(faceEdges);
            }

            // link all edges to their pairs
            EdgeLinker.LinkEdgePairs(ref edges);

            // iterate over all edges and add to kernel
            foreach (var edge in edges)
            {
                kernel.Insert(edge);
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
