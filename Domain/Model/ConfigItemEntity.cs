using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OSCiR.Model
{
    public class ConfigItemEntity : BaseEntity
    {
        //[Index("IX_UniqueKeyInt", IsUnique = true, Order = 1)]
        public string Name { get; set; } //unique
        public string Comments { get; set; }
        public string ConcreteReference { get; set; } //actual key from autodiscovery

        // Gees this is a messy of handling JSON column types
        [JsonIgnore] //don't show to the end user, this is just for data storage
        [Column("Properties", TypeName = "json")]
        public string PropertiesInternal
        {
            get
            {
                return JsonConvert.SerializeObject(Properties);
            }
            set
            {
                Properties = JsonConvert.DeserializeObject<JObject>(value);
            }
        }
        [NotMapped]
        public JObject Properties { get; set; }


        [Required]
        public Guid ClassEntityId { get; set; }
        [JsonIgnore]
        [ForeignKey("ClassEntityId")]
        public ClassEntity ParentClassEntity { get; set; }

        [Required]
        public Guid OwnerId { get; set; }
        [JsonIgnore]
        [ForeignKey("OwnerId")]
        public OwnerEntity Owner { get; set; }


        public List<ConfigItemRelationshipEntity> SourceRelationships { get; set; }
        public List<ConfigItemRelationshipEntity> TargetRelationships { get; set; }


        /*
        /// <summary>
        /// Loops though properties and remove any that don't appear in the class definition
        /// </summary>
        /// <param name="classEntity">Class entity.</param>
        internal void ProcessProperties(ClassEntity classEntity)
        {
            foreach (var prop in new JObject(this.Properties))
            {
                var classProperty = classEntity.GetProperty(prop.Key);
                if (classProperty != null) //if has the named property
                {
                    // set the key as per the class eg yourName vs yourname vs YourNAME
                    JProperty localProp = this.Properties.Property(prop.Key);
                    localProp.Value.Rename(classProperty.InternalName);
                }
                else
                {
                    if(!classEntity.AllowAnyData) this.Properties.Remove(prop.Key);
                }
            }
        }*/
    }
}
