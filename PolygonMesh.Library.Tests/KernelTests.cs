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
    public class KernelTests
    {
        [TestMethod]
        public void AddNewFace_ShouldWork()
        {
            // Arrange
            var locations = new[]
            {
                new Vec3d(0, 0, 0),
                new Vec3d(1, 1, 0),
                new Vec3d(-1, 1, 0)
            };
            var kernel = new Kernel();

            // Act
            kernel.AddNewFace(locations);
            var verts = kernel.GetFaceVertices(0).ToArray();

            // Assert
            Assert.AreEqual(1, kernel.FaceCount);
            Assert.AreEqual(3, kernel.HalfEdgeCount);
            Assert.AreEqual(3, verts.Length);
        }
    }
}
