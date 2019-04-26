using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace OSCiR.Model
{
    public class ConfigItemRelationshipEntity : BaseEntity
    {
        public Guid SourceConfigItemEntityId { get; set; }
        [JsonIgnore]
        [ForeignKey("SourceConfigItemEntityId")]
        public ConfigItemEntity SourceConfigItem { get; set; }

        public string RelationshipDescription { get; set; } //defines source to target relationship
        //public string InverseRelationshipDescription { get; set; } //defines target to source relationship

        public Guid TargetConfigItemEntityId { get; set; }
        [JsonIgnore]
        [ForeignKey("TargetConfigItemEntityId")]
        public ConfigItemEntity TargetConfigItem { get; set; }



        /*
        public Guid ClassRelationshipEntityId { get; set; }
        [ForeignKey("ClassRelationshipEntityId")]
        public ClassRelationshipEntity ClassRelationship { get; set; }
        */

        public ConfigItemRelationshipEntity()
        {
        }
    }
}
