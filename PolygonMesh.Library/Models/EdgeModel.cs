using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Models
{
    public class EdgeModel : BaseModel
    {
        public VertexModel Origin { get; set; }
        public VertexModel Target { get; set; }
    }
}
