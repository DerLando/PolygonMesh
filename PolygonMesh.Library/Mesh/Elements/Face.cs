using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Mesh.Elements
{
    /// <summary>
    /// A polygonal Face in a Mesh. Faces can have from 3 to unlimited amounts of Vertices.
    /// </summary>
    public class Face
    {
        /// <summary>
        /// The starting half-edge of this face. This is arbitrary, as a face has no 'real' start
        /// </summary>
        public HalfEdge Start { get; set; }
    }
}
