using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace PolygonMesh.IO.FileObj
{
    public static class SimpleExporter
    {
        private const char DELIMITER = ' ';
        private const char VERTEX = 'v';
        private const char FACE = 'f';
        private const char COMMENT = '#';
        
        private static string WriteVertex(VertexModel vertex)
        {
            return $"{VERTEX}{DELIMITER}" +
                $"{vertex.Position.X.ToString(CultureInfo.InvariantCulture)}{DELIMITER}" +
                $"{vertex.Position.Y.ToString(CultureInfo.InvariantCulture)}{DELIMITER}" +
                $"{vertex.Position.Z.ToString(CultureInfo.InvariantCulture)}";
        }

        private static string WriteFace(FaceModel face)
        {
            var result = $"{FACE}";
            foreach (var vertex in face.Vertices)
            {
                result += $"{DELIMITER}{vertex.Index + 1}";
            }

            return result;
        }

        private static string[] WriteMesh(Mesh mesh)
        {
            List<string> lines = new List<string>();
            lines.Add($"{COMMENT}{DELIMITER}PolygonMesh");
            lines.Add("");

            foreach (var vertex in mesh.Vertices)
            {
                lines.Add(WriteVertex(vertex));
            }

            foreach (var face in mesh.Faces)
            {
                lines.Add(WriteFace(face));
            }

            return lines.ToArray();
        }

        public static void Export(string path, Mesh mesh)
        {
            File.WriteAllLines(path, WriteMesh(mesh));
        }
    }
}
