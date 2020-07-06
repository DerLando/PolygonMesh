using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PolygonMesh.Library;
using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Mesh.Elements;

namespace PolygonMesh.RhinoPlugIn
{
    public static class PolygonMeshConversionExtensions
    {
        public static Mesh ToPolygonMesh(this Rhino.Geometry.Mesh mesh)
        {
            // clean up input mesh
            mesh.Vertices.CombineIdentical(true, true);
            mesh.Vertices.CullUnused();
            mesh.UnifyNormals();
            mesh.Weld(Math.PI);

            var positions = mesh
                .TopologyVertices
                .Select(tv => new Vec3d(tv.X, tv.Y, tv.Z))
                .ToList();

            var faces = 
                mesh
                    .GetNgonAndFacesEnumerable()
                    .Select(
                        ngon => ngon.BoundaryVertexIndexList().Select(Convert.ToInt32));

            return Mesh.CreateFromPositions(positions, faces);
        }
    }
}
