using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


using Newtonsoft.Json;

namespace OSCiR.Model
{
    public class ClassEntity : BaseEntity
    {
        public string ClassName { get; set; } //unique
        public string Comments { get; set; }
        public string Category { get; set; } //VMWare, Networking, etc
        public bool IsInstantiable { get; set; } //default true, set false if to be used only as a base class (eg Rackable class)
        public bool IsPromiscuous { get; set; } //allows any relationship to or from
        public bool AllowAnyData { get; set; } //allow any data payload to be added
        //TODO Maybe connect to Owner so we can have classes specific to a customer?

        public List<ClassPropertyEntity> Properties { get; set; }
        public List<ClassExtendEntity> Extends { get; set; }

        public List<ClassRelationshipEntity> SourceRelationships { get; set; }
        public List<ClassRelationshipEntity> TargetRelationships { get; set; }


        [JsonIgnore]
        [InverseProperty("ParentClassEntity")]
        public List<ConfigItemEntity> ConfigItemEntities { get; set; }

        public ClassEntity()
        {
        }

        internal bool ContainsProperty(string key)
        {
            return GetProperty(key) != null;
        }

        public ClassPropertyEntity GetProperty(string key)
        {
            if (this.Properties == null) return null;

            foreach (var prop in this.Properties)
            {
                if (prop.InternalName == null) continue;
                if (key.ToLower().Equals(prop.InternalName.ToLower())) return prop;
            }

            return null;
        }
    }
}
