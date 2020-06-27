using Microsoft.VisualStudio.TestTools.UnitTesting;
using PolygonMesh.IO.FileObj;
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
    }
}
