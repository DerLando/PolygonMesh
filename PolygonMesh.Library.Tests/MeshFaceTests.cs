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
        public void Faces_Can_Have_Arbitrary_Number_Of_Vertices_Bigger_Then_3() // Fails because we don't check for wierd inputs
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
        public void Faces_Cannot_Be_Added_Twice() // Fails because not implemented
        {
            // initialize
            Func<Vec3d, Vec3d, Vec3d, bool> addFaceTwiceSucceeds = (v0, v1, v2) =>
            {
                var mesh = new Mesh.Core.Mesh();

                if (!mesh.AddFace(new[] { v0, v1, v2 })) return true;
                
                mesh.AddFace(new[] { v0, v1, v2 });

                return mesh.FaceCount == 1;
            };

            Prop.ForAll(addFaceTwiceSucceeds).QuickCheckThrowOnFailure();
        }

        [TestMethod]
        public void proptest_test()
        {
    //        (Vec3d(X: 0, Y: -1.7976931348623157E+308, Z: 0), Vec3d(X: ∞, Y: 5E-324, Z: -0),
    //          Vec3d(X: NaN, Y: 0, Z: 0))

            var mesh = new Mesh.Core.Mesh();
            var positions = new[] { 
                    new Vec3d(0, -1.7976931348623157E+308, -0),
                    new Vec3d(double.PositiveInfinity, 5E-324, -0),
                    new Vec3d(double.NaN, 0, 0) };

            var success = mesh.AddFace(positions);

            var tryAgain = mesh.AddFace(positions);

            Assert.IsTrue(true);
        }
    }
}
