using PolygonMesh.Library.Mesh.Core;
using PolygonMesh.Library.Mesh.Elements;
using PolygonMesh.Library.Mesh.Iterators;
using PolygonMesh.Library.Mesh.TopologyHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PolygonMesh.Library.Mesh.TopologyOperations
{
    static class KisOperation
    {
        internal static void Kis(this Kernel kernel)
        {
            var initialFaceCount = kernel.FaceCount;

            for (int i = 0; i < initialFaceCount; i++)
            {
                var face = kernel.Faces[i];

                // get face center
                // TODO: We could move the center vertex by face normal direction to build a n - pyramid
                var center = face.GetFaceCenter();

                // get vertex for center
                var centerVertex = kernel.GetVertexForPosition(center);

                var newCenter = true;

                // iterate over face edges
                foreach (var edge in new EdgeIterator(face.Start))
                {
                    // new face that will be created
                    var newFace = new Face { Start = edge };

                    // link up current edge
                    edge.Face = newFace;

                    // create an edge from center to origin of current edge
                    if(!kernel.TryGetHalfEdgeBetweenVertices(centerVertex, edge.Origin, out var incoming))
                    {
                        incoming = new HalfEdge
                        {
                            Face = newFace,
                            Origin = centerVertex
                        };
                    }

                    // create an edge from current edge target to center
                    if(!kernel.TryGetHalfEdgeBetweenVertices(edge.Target, centerVertex, out var outgoing))
                    {
                        outgoing = new HalfEdge
                        {
                            Face = newFace,
                            Origin = edge.Target,
                        };
                    }

                    // link up center vertex on first iteration
                    if (newCenter)
                    {
                        centerVertex.Outgoing = incoming;
                        newCenter = false;
                    }

                    // link up edges
                    EdgeLinker.LinkOrderedEdgeCollection(new[] { incoming, edge, outgoing });

                    // add new edges to kernel
                    kernel.Add(incoming);
                    kernel.Add(outgoing);

                    // insert new face into kernel
                    kernel.Insert(newFace);
                }

                // unlink face
                face.Start = null;
            }

            for (int i = initialFaceCount - 1; i >= 0; i--)
            {
                kernel.Remove(kernel.Faces[i]);
            }
        }
    }
}
