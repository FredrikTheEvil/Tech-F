using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechF.Configuration
{
    //ncrunch: no coverage start
    public class ClassFactoryPropertyElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return Convert.ToString(this["name"]);
            }
        }
        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get
            {
                return Convert.ToString(this["value"]);
            }
        }
    }
    [ConfigurationCollection(typeof(ClassFactoryPropertyElement), AddItemName = "Property", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ClassFactoryPropertyCollection : ConfigurationElementCollection, IEnumerable<ClassFactoryPropertyElement>
    {
        public ClassFactoryPropertyCollection()
        {
            AddElementName = "Property";
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ClassFactoryPropertyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ClassFactoryPropertyElement)element).Name;
        }

        IEnumerator<ClassFactoryPropertyElement> IEnumerable<ClassFactoryPropertyElement>.GetEnumerator()
        {
            foreach (var i in base.BaseGetAllKeys())
            {
                var o = (ClassFactoryPropertyElement)BaseGet(i);
                if (o != null)
                    yield return o;
            }
        }
        public ClassFactoryPropertyElement this[int index]
        {
            get
            {
                return (ClassFactoryPropertyElement)BaseGet(index);
            }
        }
        public new ClassFactoryPropertyElement this[string index]
        {
            get
            {
                return (ClassFactoryPropertyElement)BaseGet(index);
            }
        }
    }

    public class ClassFactoryElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return Convert.ToString(this["type"]);
            }
        }
        [ConfigurationProperty("factoryType", IsRequired = true)]
        public string FactoryType
        {
            get
            {
                return Convert.ToString(this["factoryType"]);
            }
        }
        [ConfigurationProperty("persistPerContext", DefaultValue = false, IsRequired = false)]
        public bool PersistsPerContext
        {
            get
            {
                return Convert.ToBoolean(this["persistPerContext"]);
            }
        }
        [ConfigurationProperty("FactoryProperties", IsRequired = false)]
        public ClassFactoryPropertyCollection FactoryProperties
        {
            get
            {
                return (ClassFactoryPropertyCollection)this["FactoryProperties"];
            }
        }
    }
    [ConfigurationCollection(typeof(ClassFactoryElement), AddItemName = "Factory", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class ClassFactoryCollection : ConfigurationElementCollection, IEnumerable<ClassFactoryElement>
    {
        public ClassFactoryCollection()
        {
            AddElementName = "Factory";
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ClassFactoryElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ClassFactoryElement)element).Type;
        }

        public ClassFactoryElement this[int index]
        {
            get
            {
                return (ClassFactoryElement)BaseGet(index);
            }
        }
        public new ClassFactoryElement this[string index]
        {
            get
            {
                return (ClassFactoryElement)BaseGet(index);
            }
        }

        IEnumerator<ClassFactoryElement> IEnumerable<ClassFactoryElement>.GetEnumerator()
        {
            var keys = BaseGetAllKeys();
            foreach (var key in keys)
            {
                var i = (ClassFactoryElement)BaseGet(key);
                if (i != null)
                    yield return i;
            }

        }
    }
    public class ClassFactorySection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = false, IsDefaultCollection = true)]
        public ClassFactoryCollection ClassFactories
        {
            get
            {
                return (ClassFactoryCollection)this[""];
            }
        }
    }
    //ncrunch: no coverage end
}
