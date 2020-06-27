using PolygonMesh.Library.Mesh.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PolygonMesh.IO.FileObj
{
	enum LineType
	{
		Vertex,
		Face,
		Unknown
	}

	/// <summary>
	/// Super simple .obj parser, that only cares about vertices and faces
	/// </summary>
    public class SimpleParser
    {
		/// <summary>
		/// Parse a .obj file into the stored vertices and faces
		/// </summary>
		/// <param name="path"></param>
		/// <param name="faces"></param>
		/// <returns></returns>
        public static Vec3d[] Parse(string path, out int[][] faces)
        {
			try
			{
				List<Vec3d> vecs = new List<Vec3d>();
				List<int[]> facesList = new List<int[]>();
				foreach (var line in File.ReadLines(path))
				{
					var type = GetLineType(line);

					switch (type)
					{
						case LineType.Vertex:
							vecs.Add(ConvertToVec(line));
							break;
						case LineType.Face:
							facesList.Add(ConvertToFace(line));
							break;
						case LineType.Unknown:
							break;
						default:
							break;
					}
				}

				faces = facesList.ToArray();
				return vecs.ToArray();
			}
			catch (Exception)
			{

				throw;
			}
		}

		private static LineType GetLineType(string line)
		{
			if (string.IsNullOrEmpty(line)) return LineType.Unknown;

			if (line[0] == 'v') return LineType.Vertex;
			if (line[0] == 'f') return LineType.Face;

			return LineType.Unknown;
		}

		private static Vec3d ConvertToVec(string line)
		{
			var parts = line.Split(' ');
			var x = double.Parse(parts[1]);
			var y = double.Parse(parts[2]);
			var z = double.Parse(parts[3]);

			return new Vec3d(x, y, z);
		}

		private static int[] ConvertToFace(string line)
		{
			var parts = line.Split(' ');
			return (from part in parts.Skip(1) select int.Parse(part)).ToArray();
		}
	}
}
