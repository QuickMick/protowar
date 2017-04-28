using Newtonsoft.Json.Linq;
using Protoedit.helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protoedit.project
{
    /// <summary>
    /// Singleton
    /// </summary>
    public class UserProjectLibrary
    {
        private static UserProjectLibrary instance = new UserProjectLibrary();

        private UserProjectLibrary() { }

        public static UserProjectLibrary Instance { get { return instance; } }

        public void Init(string projectpath)
        {
            this.ProjectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "resources");

            //TODO: copy defaults;
            JArray a = null;
            using (StreamReader r = new StreamReader(Path.Combine(this.ProjectPath,"typeName_default.json")))
            {
                a = JArray.Parse(r.ReadToEnd());
            }

            foreach (var v in a)
            {
                this.attributeTypes.Add(v.ToString());
            }

           
        }

        /// <summary>
        /// Holds every possible attribute type.
        /// e.g. string,int, float
        /// 
        /// the attributeconverters convert attributetypes to  real attributes
        /// </summary>
        private ObservableCollection<string> attributeTypes = new ObservableCollection<string>();
        public ObservableCollection<string> AttributeTypes { get { return this.attributeTypes; } }

        /// <summary>
        /// these converters convert string types and values to real values,
        /// e.g. type="float", string="3.1" so it shall return a float with the value 3.1f
        /// </summary>
        private Dictionary<string, Delegate> paramConverters = new Dictionary<string, Delegate>();
        public Delegate GetParamConverter(string name)
        {
            return this.paramConverters[name];
        }

        public void AddParamConverter(string type, Delegate d)
        {
            this.paramConverters.Add(type,d);
        }

        public string ProjectPath { get; set; }
    }

    public class AttributeDefinition : NotifyPropertyChanged
    {
        private SingleValueBindingProperty<string> _name = new SingleValueBindingProperty<string>();
        public SingleValueBindingProperty<string> Name { get { return this._name; } }

        /// <summary>
        /// holds type as string and the name of the value as string
        /// </summary>
      /*  private Dictionary<string, SingleValueBindingProperty<string>> _attributes = new Dictionary<string, SingleValueBindingProperty<string>>();
        public Dictionary<string, SingleValueBindingProperty<string>> Attributes { get; }*/

        private List<dynamic> _attributes = new List<dynamic>();
        public List<dynamic> Attributes { get { return this._attributes; } }

        public dynamic AddAttribute(int index)
        {
            var newAttribute =  new { TypeName = new SingleValueBindingProperty<string>(), AttributeName = new SingleValueBindingProperty<string>(), Index = index };
            this.Attributes.Add(newAttribute);
            return newAttribute;
        }
    }
}
