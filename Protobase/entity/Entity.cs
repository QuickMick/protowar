using Microsoft.Xna.Framework;
using Protobase.manager;
using Protobase.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobase.entity
{
    //keine enumswegen ableitungen
    public class EntityType
    {
        public const int A = 1;
        public const int B = 2;
        public const int C = 3;
    }

    public class EntityClass
    {

    }

    public class Flag 
    {

    }

    public sealed class Entity // : EntityQueryContainer
    {
        #region fields
        private readonly string ID;

        public bool IsInitialized { get; private set; }

        #region constructable properties
        private Material material = null;
        public Material Material
        {
            get
            { 
                return this.material;
            }
            set
            {
                if (this.material != null)
                {
                    this.material.Destruct();
                }

                this.material = value;
                this.material.Construct(this);
            }
        }

        private Transformation transformation = null;
        public Transformation Transform
        {
            get
            {
                return this.transformation;
            }

           /* private set
            {
                if (this.transformation != null)
                {
                    this.transformation.Destruct();
                }
                this.transformation = value;
                this.transformation.Construct(this);
            }*/
        }
        private EntityMesh mesh = null;
        public EntityMesh Mesh
        {
            get
            {
                return this.mesh;
            }
            set
            {
                if (this.transformation != null)
                {
                    this.transformation.Destruct();
                }
                if (this.mesh != null)
                {
                    this.mesh.Destruct();
                }
                
                this.mesh = value;
                this.mesh.Construct(this);

                this.transformation = new Transformation();
                this.transformation.Construct(this);
            }
        }
        #endregion constructable properties


        private List<Type> behaviours = new List<Type>();
        private Dictionary<Type, EntityBehaviour> behaviourInstances = new Dictionary<Type, EntityBehaviour>();

        private Dictionary<Type, EntityAttribute> attributes = new Dictionary<Type, EntityAttribute>();

        private List<Entity> children = new List<Entity>();

        public Entity Parent { get; private set; }

        public string Type { get; set; }
        public string[] Classes { get; set; }


        private HashSet<string> flags = new HashSet<string>();

        #endregion fields

        private SceneContext sceneContext;
        public Entity()
        {
            this.ID = GuidHelper.GetUniqueGuid().ToString();
        }

      
/*
        public void Construct(SceneContext context)
        {
            this.context = context;
            this.IsInitialized = true;
        }*/

        public void Destruct()
        {
            foreach (Type eb in this.behaviours)
            {
                this.behaviourInstances[eb].Destruct(this);
            }
            this.IsInitialized = false;
        }

        #region operand overloads
        public static Entity operator +(Entity c1, Entity x)
        {
            c1.children.Add(x);
            x.Parent = c1;
            return c1;
        }

        public static Entity operator -(Entity c1, Entity x)
        {
            c1.children.Remove(x);
            x.Parent = null;
            return c1;
        }
        #endregion operand overloads

        #region flags
        public void AddFlag(string s)
        {
            this.flags.Add(s);
        }

        public void RemoveFlag(string s)
        {
            this.flags.Remove(s);
        }

        public bool HasFlag(string s)
        {
            return this.flags.Contains(s);
        }

        internal void ClearFlags()
        {
            this.flags.Clear();
        }
        #endregion flags

        #region query
        public IEnumerable<Entity> queryByClass(params string[] classes)
        {
            List<Entity> result = new List<Entity>();
            foreach (Entity e in this.children)
            {
                bool allin = true;
                foreach (string ec in classes)
                {
                    if (!e.Classes.Contains(ec))
                    {
                        allin = false;
                        break;
                    }
                }
                if (allin)
                {
                    result.Add(e);
                }

                result.AddRange(e.queryByClass(classes));
            }

            return result;
        }
        #endregion query

        
        public void Update(SceneContext c,GameTime gt)
        {
            foreach (Type t in this.behaviours)
            {
                EntityBehaviour eb = this.behaviourInstances[t];
                if (eb.IsEnabled)
                {
                    eb.Update(c, this, gt);
                }
            }

            foreach (Entity e in this.children)
            {
                e.Update(c,gt);
            }

            if (this.mesh != null)
            {
                this.mesh.Update(gt);
            }

            if (this.material != null)
            {
                this.material.Update(gt);
            }
        }

        public void Render(SceneContext g)
        {
            if (this.mesh == null) // || !this.mesh.IsVisible)
            {
                return;
            }

            this.mesh.DebugRender(g);
            if (this.mesh != null)
            {
                this.material.Render(g);
            }
        }


        #region behaviours
        public bool HasBehaviour<T>()
        {
            return this.behaviours.Contains(typeof(T));
        }

        public void AddBehaviour<T>() where T : EntityBehaviour, new()
        {
            if (this.HasBehaviour<T>())
            {
                throw new Exception("Behaviour already added!");
            }

            Type t = typeof(T);
            this.behaviours.Add(t);
            EntityBehaviour eb = new T();
            this.behaviourInstances.Add(t,eb);

            eb.Construct(this);
        }

        internal void AddBehaviour(EntityBehaviour eb)
        {
            Type t = eb.GetType();

            if (this.behaviours.Contains(t))
            {
                throw new Exception("Behaviour already added!");
            }

            this.behaviours.Add(t);
            this.behaviourInstances.Add(t, eb);

            eb.Construct(this);
        }

        public T GetBehaviour<T>() where T : EntityBehaviour
        {
            if (!this.HasBehaviour<T>())
            {
                throw new Exception("Behaviour not found!");
            }
            return (T) this.behaviourInstances[typeof(T)];
        }

        public void RemoveBehaviour<T>() where T : EntityBehaviour
        {
            Type t = typeof(T);
            this.behaviours.Remove(t);
            this.behaviourInstances.Remove(t);
        }
        #endregion behaviours

        #region attributes
        public T GetAttribute<T>() where T : EntityAttribute
        {
            if (!this.HasAttribute<T>())
            {
                throw new Exception("Attribute not found");
            }
            return (T)this.attributes[typeof(T)];
        }

        public bool HasAttribute<T>() where T : EntityAttribute
        {
            return this.attributes.ContainsKey(typeof(T));
        }

        public void AddAttribute(EntityAttribute t)
        {
            if(this.attributes.ContainsKey(t.GetType())){
                throw new Exception("attributetype already added");
            }
            this.attributes.Add(t.GetType(), t);
        }

        public void RemoveAttribute(Type t)
        {
            this.attributes.Remove(t);
        }
        #endregion attributes


       
    }

    //public abstract class EntityQueryContainer
    //{
    //    //TODO: direktesuche kein vorsortieren
    // /*   private Dictionary<EntityClass, List<Entity>> sortedEntities = new Dictionary<EntityClass, List<Entity>>();


    //    private void addEntity(Entity e)
    //    {


    //    }


    //    private List<Entity> children = new List<Entity>();

    //    public Entity Parent { get; private set; }

    //    public EntityType Type { get; set; }
    //    public EntityClass[] Classes { get; set; }

    //    public IEnumerable<Entity> QueryClass(string query)
    //    {


    //        yield return null;
    //    }


    //    public void AddClass(EntityClass ec)
    //    {

    //        if (!this.sortedEntities.ContainsKey(ec))
    //        {

    //        }
            
            

    //    }*/

    //}
}
