using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Protobase.resources
{

    public interface IResourceManager
    {
        ContentManager Content { get; }
        Texture2D GetTexture(string p);

        void LoadContentFromFile();

        bool ContentLoaded { get; }
    }


    public class PWResourceManager : IResourceManager
    {
        public ContentManager Content { get; private set; }

        private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        private IServiceProvider serviceProvider;

        private bool isDisposed = false;

        public bool ContentLoaded { get; private set; }

        /// <summary>
        /// fuer die shared instanz
        /// </summary>
        /// <param name="cm"></param>
        internal PWResourceManager(ContentManager cm) {
            this.Content = cm;
            //this.Content.RootDirectory = Path.Combine(AppDomain.CurrentDomain, "Content");
            this.serviceProvider = this.Content.ServiceProvider;
        }

        /// <summary>
        /// fuer die szenen instanzen
        /// </summary>
        /// <param name="sp"></param>
        internal PWResourceManager(IServiceProvider sp)
        {
            this.serviceProvider = sp;
     
            this.isDisposed = false;
            this.ContentLoaded = false;
        }

        public void Create()
        {
            this.UnloadContent();
            this.Content = new ContentManager(this.serviceProvider);
            this.Content.RootDirectory = "Content";
            this.isDisposed = false;
            this.ContentLoaded = false;
        }

       

        public void LoadContentFromFile()
        {
            
            TextureContent content = null;
            //load the contentFile
            using (StreamReader r = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.Content.RootDirectory, Protobase.Properties.Resources.ResourceManager.GetString("ResourcesJSON"))))
            {https://www.facebook.com/groups/410048605861429/510936412439314/?notif_t=group_activity&notif_id=1473183757529463#
                content = JsonConvert.DeserializeObject<TextureContent>(r.ReadToEnd());
            }

            foreach(string s in content.TextureNames){
                this.textures.Add(s,this.Content.Load<Texture2D>(content.TextureDirectory+@"\"+s+".png"));
            }

            this.ContentLoaded = true;

        }

        public Texture2D GetTexture(string p)
        {
            return this.textures[p];
        }


        public void UnloadContent()
        {
            if (!this.isDisposed && this.Content != null && this.ContentLoaded)
            {
                this.Content.Unload();
                this.textures = new Dictionary<string, Texture2D>();
                this.ContentLoaded = false;
            }

            this.isDisposed = true;
        }
    }

    
    class TextureContent
    {
        public String TextureDirectory=null;
        public String[] TextureNames=null;

    }
}
