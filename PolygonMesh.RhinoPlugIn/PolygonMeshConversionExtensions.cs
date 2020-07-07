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

        public static Rhino.Geometry.Mesh ToRhinoMesh(this Mesh source)
        {
            // empty rhino mesh
            var rMesh = new Rhino.Geometry.Mesh();

            // add vertices
            foreach (var vertex in source.Vertices)
            {
                rMesh.Vertices.Add(vertex.Position.X, vertex.Position.Y, vertex.Position.Z);
            }

            // add faces
            foreach (var face in source.Faces)
            {
                switch (face.Vertices.Length)
                {
                    case 3:
                        rMesh.Faces.AddFace(face.Vertices[0].Index, face.Vertices[1].Index, face.Vertices[2].Index);
                        break;
                    case 4:
                        rMesh.Faces.AddFace(face.Vertices[0].Index, face.Vertices[1].Index, face.Vertices[2].Index, face.Vertices[3].Index);
                        break;
                    default:
                        // triangulate about face center (fan)
                        var center = Vec3d.Average(face.Vertices.Select(v => v.Position).ToList()).ToPoint3d();
                        rMesh.Vertices.Add(center);

                        var faceIndices = new int[face.Vertices.Length];

                        for (int i = 0; i < face.Vertices.Length; i++)
                        {
                            faceIndices[i] =
                                rMesh.Faces
                                .AddFace(
                                    face.Vertices[i].Index,
                                    face.Vertices[(i + 1) % face.Vertices.Length].Index,
                                    rMesh.Vertices.Count - 1);
                        }

                        rMesh.Ngons.AddNgon(Rhino.Geometry.MeshNgon.Create(face.Vertices.Select(v => v.Index).ToList(), faceIndices));
                        break;
                }
            }

            // clean up
            rMesh.Normals.ComputeNormals();
            return rMesh;
        }

        public static Rhino.Geometry.Point3d ToPoint3d(this Vec3d vector)
        {
            return new Rhino.Geometry.Point3d(vector.X, vector.Y, vector.Z);
        }
    }
}
