using FsCheck;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolygonMesh.Library.Tests
{
    [TestClass]
    public class MeshFaceTests
    {

        [TestMethod]
        public void Faces_Cannot_Have_1_Vertex()
        {
            // initialize
            var mesh = new Mesh.Core.Mesh();
            Func<Vec3d, bool> addSingularFaceSucceeds = v =>
                {
                    mesh.AddFace(new[] { v });
                    return mesh.FaceCount == 0;
                };

            Prop.ForAll(addSingularFaceSucceeds).QuickCheckThrowOnFailure();
        }

        [TestMethod]
        public void Faces_Cannot_Have_2_Vertices()
        {
            // initialice
            var mesh = new Mesh.Core.Mesh();
            Func<Vec3d, Vec3d, bool> addLineFaceSucceeds = (v0, v1) =>
            {
                mesh.AddFace(new[] { v0, v1 });
                return mesh.FaceCount == 0;
            };

            Prop.ForAll(addLineFaceSucceeds).QuickCheckThrowOnFailure();
        }

        [TestMethod]
        public void Faces_Can_Have_Arbitrary_Number_Of_Vertices_Bigger_Then_3()
        {
            Func<Vec3d[], bool> addFaceSucceeds = vs =>
            {
                // prop tests before this ensure the early exit here is sound
                if (vs.Length < 3) return true;

                var mesh = Mesh.Core.Mesh.CreateSingleFace(vs);

                return (mesh.FaceCount == 1) && (mesh.HalfEdgeCount == vs.Length);
            };

            Prop.ForAll(addFaceSucceeds).QuickCheckThrowOnFailure();

        }

        [TestMethod]
        public void Faces_Cannot_Be_Added_Twice()
        {
            // initialize
            Func<Vec3d, Vec3d, Vec3d, bool> addFaceTwiceSucceeds = (v0, v1, v2) =>
            {
                var mesh = new Mesh.Core.Mesh();

                mesh.AddFace(new[] { v0, v1, v2 });
                mesh.AddFace(new[] { v0, v1, v2 });

                return mesh.FaceCount == 1;
            };

            Prop.ForAll(addFaceTwiceSucceeds).QuickCheckThrowOnFailure();

        }
    }
}
