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
        public void SimpleQuad_ShouldHaveValidTopology()
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

            // Assert
            Assert.AreEqual(4, mesh.VertexCount);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(2, mesh.GetVertexNeighbours(i).Count);
            }
        }

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
            Assert.AreEqual(10, mesh.HalfEdgeCount);
            SimpleExporter.Export("D:\\Git\\PolygonMesh\\PolygonMesh.Library.Tests\\Resources\\test.obj", mesh);
        }

        [TestMethod]
        public void SplitFace_ShouldWorkForComplicatedFile()
        {
            // Arrange
            var path = "D:\\Git\\PolygonMesh\\PolygonMesh.Library.Tests\\Resources\\torus.obj";
            var vecs = SimpleParser.Parse(path, out var faces);
            var mesh = Mesh.Core.Mesh.CreateFromPositions(vecs, faces);

            // Act
            var initialFaces = mesh.FaceCount;
            var initialVertices = mesh.VertexCount;
            for (int i = 0; i < initialFaces; i++)
            {
                mesh.SplitFace(i);
            }

            // Assert
            Assert.AreEqual(initialVertices, mesh.VertexCount);
            Assert.AreEqual(initialFaces * 2, mesh.FaceCount);
            SimpleExporter.Export("D:\\Git\\PolygonMesh\\PolygonMesh.Library.Tests\\Resources\\test.obj", mesh);
        }

        [TestMethod]
        public void SplitEdge_ShouldWorkOnSimpleQuad()
        {
            // Arrange
            var positions = new[]
            {
                new Vec3d(0, 0, 0),
                new Vec3d(1, 0, 0),
                new Vec3d(1, 1, 0),
                new Vec3d(0, 1, 0),
            };
            var rand = new Random();
            Mesh.Core.Mesh mesh = null;

            for (int i = 0; i < 100; i++)
            {
                mesh = Mesh.Core.Mesh.CreateSingleFace(positions);

                // Act
                var index = rand.Next(0, 4);
                var param = rand.Next(2, 8) / 10.0;
                mesh.SplitEdge(index, param, out _);

                // Assert
                Assert.AreEqual(1, mesh.FaceCount);
                Assert.AreEqual(5, mesh.VertexCount);
                Assert.AreEqual(10, mesh.HalfEdgeCount);

            }

            SimpleExporter.Export("D:\\Git\\PolygonMesh\\PolygonMesh.Library.Tests\\Resources\\test.obj", mesh);
        }

        [TestMethod]
        public void CollapseEdge_Should_Work_On_Simple_Quad() // How would a collapsed edge on a quad look like?
        {
            // Arrange
            var positions = new[]
            {
                new Vec3d(0, 0, 0),
                new Vec3d(1, 0, 0),
                new Vec3d(1, 1, 0),
                new Vec3d(0, 1, 0),
            };
            var rand = new Random();
            Mesh.Core.Mesh mesh = null;

            for (int i = 0; i < 4; i++)
            {
                mesh = Mesh.Core.Mesh.CreateSingleFace(positions);

                // Act
                var index = rand.Next(0, 4);
                mesh.CollapseEdge(0, index);

                // Assert
                Assert.AreEqual(1, mesh.FaceCount);
                Assert.AreEqual(3, mesh.VertexCount);
                Assert.AreEqual(6, mesh.HalfEdgeCount);

            }

            SimpleExporter.Export("D:\\Git\\PolygonMesh\\PolygonMesh.Library.Tests\\Resources\\test.obj", mesh);
        }

        [TestMethod]
        public void SplitFace_And_CollapseEdge_Are_Inverse()
        {
            // Arrange
            var positions = new[]
            {
                new Vec3d(0, 0, 0),
                new Vec3d(1, 0, 0),
                new Vec3d(1, 1, 0),
                new Vec3d(0, 1, 0),
            };
            var rand = new Random();
            Mesh.Core.Mesh mesh = null;

            for (int i = 0; i < 4; i++)
            {
                mesh = Mesh.Core.Mesh.CreateSingleFace(positions);

                // Act
                var index = rand.Next(0, 4);
                mesh.SplitFace(0, index, (index + 2) % 4, out _);
                mesh.CollapseEdge(1, 2);

                // Assert
                Assert.AreEqual(1, mesh.FaceCount);
                Assert.AreEqual(4, mesh.VertexCount);
                Assert.AreEqual(8, mesh.HalfEdgeCount);
            }
        }

        [TestMethod]
        public void SplitFace_And_SplitEdge_Together_Can_Turn_Simple_Quad_Into_Two_Rectangles()
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
            mesh.SplitEdge(0, 0, 0.5, out _);
            mesh.SplitEdge(0, 3, 0.5, out _);
            mesh.SplitFace(0, 1, 4, out _);

            // Assert
            //Assert.AreEqual(6, mesh.VertexCount);
            //Assert.AreEqual(14, mesh.HalfEdgeCount);
            //Assert.AreEqual(2, mesh.FaceCount);
            SimpleExporter.Export("D:\\Git\\PolygonMesh\\PolygonMesh.Library.Tests\\Resources\\test.obj", mesh);
        }

        [TestMethod]
        public void SplitFace_And_SplitEdge_Together_Can_Create_Complex_Mesh()
        {
            // building the mesh from this: https://w3.cs.jmu.edu/bowersjc/page/courses/spring17/cs480/labs/images/final_dcel.png

            // Arrange
            var positions = new[]
            {
                new Vec3d(0, 0, 0),
                new Vec3d(2, 0, 0),
                new Vec3d(2, 1, 0),
                new Vec3d(0, 1, 0),
            };
            Mesh.Core.Mesh mesh = Mesh.Core.Mesh.CreateSingleFace(positions);

            // Act
            // split in half
            mesh.SplitEdge(0, 0, 0.5, out var firstEdgeIndices);
            mesh.SplitEdge(0, 3, 0.5, out var secondEdgeIndices);
            mesh.SplitFace(0, firstEdgeIndices.Item2, secondEdgeIndices.Item2, out _);

            // split right quad in two triangles
            mesh.SplitFace(0, 0, 2, out _);

            // split left quad in three triangles
            mesh.SplitEdge(1, 0, 0.5, out firstEdgeIndices);
            mesh.SplitFace(1, firstEdgeIndices.Item2, 3, out var facePartIndices);
            mesh.SplitFace(facePartIndices.Item2, 1, 3, out facePartIndices);

            // split middle triangle of split left quad in half
            mesh.SplitEdge(facePartIndices.Item2, 0, 0.5, out firstEdgeIndices);
            mesh.SplitEdge(facePartIndices.Item2, 3, 0.5, out secondEdgeIndices);
            mesh.SplitFace(facePartIndices.Item2, firstEdgeIndices.Item2, secondEdgeIndices.Item2, out facePartIndices);

            // split lower quad of split triangle in half
            mesh.SplitEdge(facePartIndices.Item1, 0, 0.5, out firstEdgeIndices);
            mesh.SplitEdge(facePartIndices.Item1, 3, 0.5, out secondEdgeIndices);
            mesh.SplitFace(facePartIndices.Item1, firstEdgeIndices.Item2, secondEdgeIndices.Item2, out _);

            // Assert
            Assert.AreEqual(11, mesh.VertexCount);
            Assert.AreEqual(7, mesh.FaceCount);
            Assert.AreEqual(38, mesh.HalfEdgeCount); // i think it should be 34 instead
            SimpleExporter.Export("D:\\Git\\PolygonMesh\\PolygonMesh.Library.Tests\\Resources\\test.obj", mesh);
        }

    }
}
