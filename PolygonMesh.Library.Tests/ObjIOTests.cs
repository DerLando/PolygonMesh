using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolygonMesh.IO.FileObj;
using PolygonMesh.Library.Mesh.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PolygonMesh.Library.Tests
{
    [TestClass]
    public class ObjIOTests
    {
        [TestMethod]
        public void SimpleParser_ShouldParse()
        {
            // Arrange
            var path = "D:\\Git\\PolygonMesh\\PolygonMesh.Library.Tests\\Resources\\cube.obj";

            // Act
            var vecs = SimpleParser.Parse(path, out var faces);

            // Assert
            Assert.AreEqual(8, vecs.Length);
            Assert.AreEqual(6, faces.Length);
        }

        [TestMethod]
        public void SimpleParser_ParseShouldCreateKernel()
        {
            // Arrange
            var path = "D:\\Git\\PolygonMesh\\PolygonMesh.Library.Tests\\Resources\\cube.obj";

            // Act
            var vecs = SimpleParser.Parse(path, out var faces);
            var mesh = Mesh.Core.Mesh.CreateFromPositions(vecs, faces);

            // Assert
            Assert.AreEqual(vecs.Length, mesh.VertexCount);
            Assert.AreEqual(faces.Length, mesh.FaceCount);
            Assert.AreEqual(24, mesh.HalfEdgeCount);
        }
    }
}
