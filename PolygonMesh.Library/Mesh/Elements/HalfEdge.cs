using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Elements
{
    /// <summary>
    /// A Half-Edge representation of a Mesh edge. A HalfEdge is directed, going from a start <see cref="Vertex"/> <see cref="Origin"/>,
    /// to a target <see cref="Vertex"/> <see cref="Target"/>.
    /// A Half-edge also stores references to the next and previous edges linked to it, as well as its Pair and the face it belongs to.
    /// </summary>
    public class HalfEdge
    {
        /// <summary>
        /// First Vertex along the Face of the HalfEdge
        /// </summary>
        public Vertex Origin { get; set; }

        /// <summary>
        /// Next HalfEdge after this along the Face
        /// </summary>
        public HalfEdge Next { get; set; }

        /// <summary>
        /// Previous HalfEdge before this along the Face
        /// </summary>
        public HalfEdge Previous { get; set; }

        /// <summary>
        /// Pair HalfEdge with inverse direction
        /// </summary>
        public HalfEdge Pair { get; set; }

        /// <summary>
        /// Face of the HalfEdge
        /// </summary>
        public Face Face { get; set; }

        #region Auto-properties

        /// <summary>
        /// Target Vertex this HalfEdge is directed to
        /// </summary>
        public Vertex Target => Pair.Origin;

        #endregion

        public override string ToString()
        {
            return $"HalfEdge from {Origin} to {Target}";
        }
    }
}
