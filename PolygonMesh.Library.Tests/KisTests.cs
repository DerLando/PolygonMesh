using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolygonMesh.IO.FileObj;
using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Tests
{
    [TestClass]
    public class KisTests
    {
        [TestMethod]
        public void Kis_Should_Work_On_Simple_Quad()
        {
            // Arrange
            var positions = new[]
            {
                new Vec3d(0, 0, 0),
                new Vec3d(1, 0, 0),
                new Vec3d(1, 1, 0),
                new Vec3d(0, 1, 0),
            };
            Mesh.Core.Mesh mesh = Mesh.Core.Mesh.CreateSingleFace(positions);

            // Act
            mesh.Kis();

            // Assert
            Assert.AreEqual(5, mesh.VertexCount);
            Assert.AreEqual(4, mesh.FaceCount);
            Assert.AreEqual(16, mesh.HalfEdgeCount);
            SimpleExporter.Export("D:\\Git\\PolygonMesh\\PolygonMesh.Library.Tests\\Resources\\test.obj", mesh);

        }
    }
}
