using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Protobase.entity;
using Protobase.manager;
using Protobase.resources;
using Protobase.util;
using Protowar;
using Protowar.scenes.gameplay.scenecomponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobwar.scenes.gameplay
{
    public class GameplayScene : Scene
    {
        public GameplayScene() : base("GAMEPLAY") { }

        private ProtoCamera cam;

        public override void BuildScene()
        {    
            this.cam = new ProtoCamera(){ Zoom =4f, EnableTracking = true, EnableRotationTracking = true, MinRotation = -0.05f, MaxRotation = 0.05f, SmoothSpeedMultiplicator = 50f };
            this.AddComponent(this.cam);
            this.AddComponent(new WorldManager());
            this.AddComponent(new GameplayComponent());

            

            //TODO: add components
        
            InputConfig.Instance.Init(this.Context.InputManager);
        }

        public override void LoadSharedContent(IResourceManager shared)
        {    
        }

        public override void OpenSpritebatch(SceneContext context)
        {
            context.GraphicsDeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            context.GraphicsDeviceManager.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            context.SpriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, this.cam.View);    
        }

        public override void OpenHudSpritebatch(SceneContext context)
        {
            context.SpriteBatch.Begin();
        }
    }
}
