using Protobase.entity;
using Protobase.entity.materials;
using Protobase.manager;
using Protobase.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobase
{
    public static class Proto
    {
        public static void Initialize(IResourceManager rm, WorldManager wm)
        {
            #region default materials
            EntityFactory.Instance.AddMaterialFactory("SimpleTexturedMaterial", (d) =>
            {
                return new SimpleTexturedMaterial(rm, d.Get<string>("textureName"));
            });
            #endregion default materials

            #region default behaviours
           
            #endregion default behaviours

            #region default meshes
            EntityFactory.Instance.AddMeshFactory("QuadMesh", (d,x,y,r) =>
            {
                return new QuadMesh(wm, d.GetFloat(QuadMesh.JSON_WIDTH), d.GetFloat(QuadMesh.JSON_HEIGHT), x, y, r);
            });

            EntityFactory.Instance.AddMeshFactory("CircleMesh", (d,x,y,r) =>
            {
                return new CircleMesh(wm, d.GetFloat(CircleMesh.JSON_RADIUS),x,y,r);
            });

            #endregion default meshes
        }

    }
}
