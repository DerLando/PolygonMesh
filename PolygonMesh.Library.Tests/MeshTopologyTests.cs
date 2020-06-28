using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolygonMesh.IO.FileObj;
using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Tests
{
    [TestClass]
    public class MeshTopologyTests
    {
        [TestMethod]
        public void SplitFace_ShouldWorkForSimpleQuad()
        {
            // Arrange
            var positions = new[]
            {
                new Vec3d(0, 0, 0),
                new Vec3d(1, 0, 0),
                new Vec3d(1, 1, 0),
                new Vec3d(0, 1, 0),
            };
            var mesh = Mesh.Core.Mesh.CreateSingleFace(positions);

            // Act
            mesh.SplitFace(0);

            // Assert
            Assert.AreEqual(2, mesh.FaceCount);
            Assert.AreEqual(4, mesh.VertexCount);
            Assert.AreEqual(6, mesh.HalfEdgeCount);
            SimpleExporter.Export("D:\\Git\\PolygonMesh\\PolygonMesh.Library.Tests\\Resources\\test.obj", mesh);
        }
    }
}
