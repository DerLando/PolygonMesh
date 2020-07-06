using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;

namespace PolygonMesh.RhinoPlugIn
{
    public class PolygonMeshRhinoPlugInCommand : Command
    {
        public PolygonMeshRhinoPlugInCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static PolygonMeshRhinoPlugInCommand Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "PolygonMeshRhinoPlugInCommand"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoApp.WriteLine("The {0} command is under construction.", EnglishName);

            if (RhinoGet.GetOneObject("Pick mesh", false, ObjectType.Mesh, out var objRef) != Result.Success)
                return Result.Failure;

            var mesh = objRef.Mesh();
            var poly = mesh.ToPolygonMesh();

            return Result.Success;
        }
    }
}
