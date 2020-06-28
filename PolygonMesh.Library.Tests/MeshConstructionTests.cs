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
    public class MeshConstructionTests
    {
        [TestMethod]
        public void SingleFace_ShouldWork()
        {
            // Arrange
            var locations = new[]
            {
                new Vec3d(0, 0, 0),
                new Vec3d(1, 1, 0),
                new Vec3d(-1, 1, 0)
            };

            // Act
            var mesh = Mesh.Core.Mesh.CreateSingleFace(locations);
            var vertices = mesh.Vertices;

            // Assert
            Assert.AreEqual(1, mesh.FaceCount);
            Assert.AreEqual(3, mesh.HalfEdgeCount);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(locations[i], mesh.Vertices[i].Position);
            }
        }
    }
}
