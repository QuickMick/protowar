using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Protobase;
using Protobase.entity;
using Protobase.manager;
using Protobwar.scenes;
using Protobwar.scenes.gameplay;
using System;
using System.Dynamic;

namespace Protowar
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {   
            using (var game = new SceneManager(1280,720))
            {
                GameplayScene gs = new GameplayScene();
                game.AddScene(gs);
                //TODO: add more scenes

                game.StartScene(gs.SceneName);
                game.Run();
            }
        }
    }



#endif
}


//TODO: settings, eventmanager für entities, hudmanager
