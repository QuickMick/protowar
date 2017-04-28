using Microsoft.Xna.Framework;
using Protobase.entity;
using Protobase.resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobase.manager
{
    public interface ISceneComponent
    {

        /// <summary>
        /// dazu da, um alle refferenzen aus dem context zu holen
        /// </summary>
        /// <param name="context"></param>
        void Initialize(SceneContext context);

        /// <summary>
        /// lädt kontent, welche für die szene benötit wird (z.b. levelcontent)
        /// </summary>
        /// <param name="shared"></param>
        /// <param name="scene"></param>
        void LoadContent(SceneContext context);

        /// <summary>
        /// startet die szene
        /// </summary>
        /// <param name="ec"></param>
        /// <param name="previous">szene die als nächstes gestartet wird</param>
        void OnStart(SceneContext ec, Scene previous=null);

        void Update(SceneContext e, GameTime t);

        void Render(SceneContext e);


        void RenderDebug(SceneContext e);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="next">szene die davor gelaufen is</param>
        void OnPause(SceneContext e,Scene next=null);

        void UnloadContent(IResourceManager scene);
    }
}
