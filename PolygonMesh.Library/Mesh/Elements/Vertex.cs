using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Elements
{
    /// <summary>
    /// A Mesh Vertex
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// A reference to one of the half-edges leaving this vertex
        /// </summary>
        public HalfEdge Outgoing { get; set; }

        /// <summary>
        /// The Position of the Vertex in 3d Space
        /// </summary>
        public Vec3d Position { get; set; }

        public override string ToString()
        {
            return $"Vertex, Position: {Position}";
        }
    }
}
