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
        public void Faces_Can_Have_Arbitrary_Number_Of_Vertices_Bigger_Then_3()
        {
            var rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                // Arrange
                var vertexCount = rand.Next(3, 100);
                var locations = 
                    from index in Enumerable.Range(0, vertexCount) 
                    select new Vec3d(rand.NextDouble(), rand.NextDouble(), rand.NextDouble());

                // Act
                var mesh = Mesh.Core.Mesh.CreateSingleFace(locations);

                // Assert
                Assert.AreEqual(1, mesh.FaceCount);
                Assert.AreEqual(vertexCount * 2, mesh.HalfEdgeCount);
            }
        }

        [TestMethod]
        public void Faces_Cannot_Have_Less_Than_3_Vertices()
        {
            // initialize
            var rand = new Random();
            var mesh = new Mesh.Core.Mesh();

            for (int i = 0; i < 100; i++)
            {
                // Arrange
                var locations =
                    from index in Enumerable.Range(0, rand.Next(0, 3))
                    select new Vec3d(rand.NextDouble(), rand.NextDouble(), rand.NextDouble());

                // Act
                mesh.AddFace(locations);

                // Assert
                Assert.AreEqual(0, mesh.FaceCount);
            }
        }

        [TestMethod]
        public void Faces_Cannot_Be_Added_Twice()
        {
            // initialize
            var rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                // Arrange
                var mesh = new Mesh.Core.Mesh();
                var locations =
                    from index in Enumerable.Range(0, rand.Next(3, 100))
                    select new Vec3d(rand.NextDouble(), rand.NextDouble(), rand.NextDouble());

                // Act
                mesh.AddFace(locations);
                mesh.AddFace(locations);

                // Assert
                Assert.AreEqual(1, mesh.FaceCount);
            }
        }
    }
}
