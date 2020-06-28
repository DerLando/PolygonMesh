using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolygonMesh.IO.FileObj;
using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Mesh.Iterators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolygonMesh.Library.Tests
{
    [TestClass]
    public class IteratorTests
    {
        [TestMethod]
        public void VertexRingIterator_ShouldWork()
        {
            // Arrange
            var path = "D:\\Git\\PolygonMesh\\PolygonMesh.Library.Tests\\Resources\\cube.obj";
            var vecs = SimpleParser.Parse(path, out var faces);
            var kernel = Kernel.CreateFromPositions(vecs, faces);

            // Act
            var ringIterator = new VertexRingIterator(kernel.GetFaceVertices(0).First());

            // Assert
            Assert.AreEqual(3, ringIterator.Count());
        }
    }
}
