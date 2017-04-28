using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Protobase.manager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Protobase.entity
{
    public delegate Material MaterialFabricator(IDictionary<string, object> args);
    public delegate EntityMesh MeshFabricator(IDictionary<string, object> args, float x, float y,float rotation);
    public delegate EntityAttribute AttributeFabricator(IDictionary<string, object> args);
   

    public class EntityFactory
    {


        #region singleton
        private static EntityFactory instance = new EntityFactory();
        public static EntityFactory Instance { get { return EntityFactory.instance; } }

        private EntityFactory()
        {
            // TODO: build type lists
        }
        #endregion singleton

        #region library
        //  private static Dictionary<string, Type> behaviourTypes = new Dictionary<string, Type>();
        #region possible types
        private Dictionary<string, MaterialFabricator> materialFabricators = new Dictionary<string, MaterialFabricator>();
        public void AddMaterialFactory(string materialName, MaterialFabricator fabricator)
        {
            this.materialFabricators.Add(materialName, fabricator);
        }

        private Dictionary<string, MeshFabricator> meshFabricators = new Dictionary<string, MeshFabricator>();
        public void AddMeshFactory(string materialName, MeshFabricator fabricator)
        {
            this.meshFabricators.Add(materialName, fabricator);
        }

        private delegate EntityBehaviour BehaviourFabricator();
        private Dictionary<string, BehaviourFabricator> behaviourFabricators = new Dictionary<string, BehaviourFabricator>();
        public void AddBehaviour<T>(string name) where T :EntityBehaviour, new()
        {
            this.behaviourFabricators.Add(name, ()=>{ return new T();});
        }

        private Dictionary<string, AttributeFabricator> attributeFabricators = new Dictionary<string, AttributeFabricator>();
        public void AddAttributeFactory(string materialName, AttributeFabricator fabricator)
        {
            this.attributeFabricators.Add(materialName, fabricator);
        }
        #endregion possible types

        #region prefabs
        private Dictionary<string, EntityDefinition> prefabs = new Dictionary<string, EntityDefinition>();

        public void AddPrefab(string name, EntityDefinition ed)
        {
            this.prefabs.Add(name, ed);
        }

        public void LoadPrefab(string file)
        {
            //TODO:
            throw new NotImplementedException();
        }

        public void AddJsonDefinition(string name, string json)
        {
            this.prefabs.Add(name, JsonConvert.DeserializeObject<EntityDefinition>(json));
        }
        #endregion prefabs
        #endregion library

        /*public string Serialize(Entity e)
        {
            //serializes with type
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

            return JsonConvert.SerializeObject(e, jsonSerializerSettings);
        }*/

        //TODO: keine dynamic mehr, dafür festes oject
        //[Obsolete]
        //public Entity GenerateDynamical(dynamic entityDescription)
        //{
        //    Entity result = new Entity();

        //    // add type
        //    if (DoesPropertyExist(entityDescription, "Type"))
        //    {
        //        result.Type = entityDescription.Type;
        //    }

        //    // add classes
        //    if (DoesPropertyExist(entityDescription, "Classes"))
        //    {
        //        result.Classes = entityDescription.Classes;
        //    }

        //    // add material if available
        //    if (DoesPropertyExist(entityDescription, "Material"))
        //    {
        //        result.Material = this.materialFabricators[entityDescription.Material.Type]((IDictionary<string, object>)entityDescription.Material);
        //    }

        //    //add Mesh if available
        //    if (DoesPropertyExist(entityDescription, "Mesh")) // entityDescription.hasProperty("test")
        //    {
        //        result.Mesh = this.meshFabricators[entityDescription.Mesh.Type](entityDescription.Mesh, entityDescription.X, entityDescription.Y, entityDescription.Rotation);
        //    }

        //    //add attributes
        //    if (DoesPropertyExist(entityDescription, "Attributes"))
        //    {
        //        foreach (dynamic s in entityDescription.Attributes)
        //        {
        //            result.AddAttribute(this.attributeFabricators[s.Type](s));
        //        }
        //    }

        //    //add Behaviours if available
        //    if (DoesPropertyExist(entityDescription, "Behaviours"))
        //    {
        //        foreach (string s in entityDescription.Behaviours)
        //        {
        //            result.AddBehaviour(this.behaviourFabricators[s]());
        //        }
        //    }

        //    // finally add children recursively
        //    if (DoesPropertyExist(entityDescription, "Children"))
        //    {
        //        foreach (dynamic s in entityDescription.Children)
        //        {
        //            result += EntityFactory.GenerateDynamical(s);
        //        }
        //    }

        //    return result;
        //}

        public Entity GenerateByName(string entityName, float x=0, float y=0, float rotation=0)
        {
            EntityDefinition ed = this.prefabs[entityName];
          /*  float xold = ed.X;
            float yold = ed.Y;
            float rotold = ed.Rotation;*/
            ed.X = x;
            ed.Y = y;
            ed.Rotation = rotation;

            Entity result = this.Generate(ed);

           /* ed.X = xold;
            ed.Y = yold;
            ed.Rotation = rotold;*/

            ed.X = 0;
            ed.Y = 0;
            ed.Rotation = 0;

            return result;

        }

        public Entity Generate(EntityDefinition entityDescription)
        {
            Entity result = new Entity();

            // add type
            result.Type = (!String.IsNullOrEmpty(entityDescription.Type)) ? entityDescription.Type : "";

            // add classes
            result.Classes = (entityDescription.Classes != null && entityDescription.Classes.Length > 0)?entityDescription.Classes:new string[]{};
     

            // add material if available
            if (entityDescription.Material != null)
            {
                result.Material = this.materialFabricators[entityDescription.Material.Get<string>(EntityDefinition.JSON_TYPE)](entityDescription.Material);
            }

            //add Mesh if available
            if (entityDescription.Mesh != null) // entityDescription.hasProperty("test")
            {
                result.Mesh = this.meshFabricators[entityDescription.Mesh.Get<string>(EntityDefinition.JSON_TYPE)](entityDescription.Mesh,entityDescription.X,entityDescription.Y,entityDescription.Rotation);
            }

            //add attributes
            if (entityDescription.Attributes != null && entityDescription.Attributes.Length >0)
            {
                foreach (Dictionary<string, object> s in entityDescription.Attributes)
                {
                    result.AddAttribute(this.attributeFabricators[s.Get<string>(EntityDefinition.JSON_TYPE)](s));
                }
            }

            //add Behaviours if available
            if (entityDescription.Behaviours != null && entityDescription.Behaviours.Length > 0)
            {
                foreach (string s in entityDescription.Behaviours)
                {
                    result.AddBehaviour(this.behaviourFabricators[s]());
                }
            }

            // finally add children recursively
            if (entityDescription.Children != null && entityDescription.Children.Length > 0)
            {
                foreach (EntityDefinition s in entityDescription.Children)
                {
                    result += this.Generate(s);
                }
            }

            return result;
        }


        private static bool hasProperty(ExpandoObject Dynamique, string Property)
        {
            IDictionary<string, object> properties = (IDictionary<string, object>)Dynamique;
            return properties.ContainsKey(Property);
        }

        private bool DoesPropertyExist(dynamic settings, string name)
        {
           // return settings.GetType().GetProperty(name) != null;
            return hasProperty(settings, name);
          //  return settings.ContainsKey(name);
        }

        public static dynamic Parse(string json)
        {
            return JsonConvert.DeserializeObject<ExpandoObject>(json, new ExpandoObjectConverter());
        }
    }

    public static class ExtensionDictionary
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(this IDictionary<string, object> t, string key)
        {
            object val = t[key];
            return t.ContainsKey(key)?(T)val:default(T);
        }

        public static float GetFloat(this IDictionary<string, object> t, string key)
        {
            return t.ContainsKey(key)?Convert.ToSingle(t[key]):0f;
        }

        public static int GetInt(this IDictionary<string, object> t, string key)
        {
            return t.ContainsKey(key) ? Convert.ToInt32(t[key]) : 0;
        }
    }


    public class EntityDefinition
    {
        #region json constants
        public const string JSON_TYPE = "type";
        public const string JSON_CLASSES = "classes";
        public const string JSON_X = "x";
        public const string JSON_Y = "y";
        public const string JSON_ROTATION = "rotation";

        public const string JSON_MATERIAL = "material";
        public const string JSON_BEHAVIOURS = "behaviours";

        public const string JSON_MESH = "mesh";
        public const string JSON_ATTRIBUTES = "attributes";
        public const string JSON_CHILDREN = "children";
        #endregion json constants

        public readonly string ID;
        public EntityDefinition()
        {
            this.ID = new Guid().ToString();
        }


        [JsonProperty(PropertyName = EntityDefinition.JSON_TYPE)]
        public string Type;

        [JsonProperty(PropertyName = EntityDefinition.JSON_CLASSES)]
        public string[] Classes;

        //startposition oder relative position
        [JsonProperty(PropertyName = EntityDefinition.JSON_X)]
        public float X=0;

        [JsonProperty(PropertyName = EntityDefinition.JSON_Y)]
        public float Y=0;

        [JsonProperty(PropertyName = EntityDefinition.JSON_ROTATION)]
        public float Rotation = 0;

        [JsonProperty(PropertyName = EntityDefinition.JSON_ATTRIBUTES)]
        public Dictionary<string, object>[] Attributes;

        [JsonProperty(PropertyName = EntityDefinition.JSON_BEHAVIOURS)]
        public string[] Behaviours;

        [JsonProperty(PropertyName = EntityDefinition.JSON_MATERIAL)]
        public Dictionary<string, object> Material;

        [JsonProperty(PropertyName = EntityDefinition.JSON_MESH)]
        public Dictionary<string, object> Mesh;

        [JsonProperty(PropertyName = EntityDefinition.JSON_CHILDREN)]
        public EntityDefinition[] Children;
    }
}


