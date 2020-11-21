using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyQueries
{
    internal static class VertexQuery
    {
        /// <summary>
        /// Creates (or finds an existing) vertex for the given position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static Vertex GetVertexForPosition(OcTree<Vertex> vertexCollection, Vec3d position)
        {
            var vertex = new Vertex { Position = position };
            if (vertexCollection.Insert(vertex, position))
                return vertex;

            return vertexCollection.FindClosest(position);
        }

        /// <summary>
        /// Creates (or finds existing) vertices for the given collection of positions.
        /// As this uses distance queries, stacked positions will only produce one vertex.
        /// The output is not guaranteed to have the same number of elements as the input.
        /// </summary>
        /// <param name="vertexCollection"></param>
        /// <param name="positions"></param>
        /// <returns></returns>
        public static IEnumerable<Vertex> GetVerticesForPositions(OcTree<Vertex> vertexCollection, IEnumerable<Vec3d> positions)
        {
            var vertices = new HashSet<Vertex>();

            foreach (var position in positions)
            {
                vertices.Add(GetVertexForPosition(vertexCollection, position));
            }

            return vertices;
        }
    }
}
