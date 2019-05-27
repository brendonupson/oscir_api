using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace OSCiR.Model
{
    public class ClassRelationshipEntity : BaseEntity
    {
        public Guid SourceClassEntityId { get; set; }
        [JsonIgnore]
        [ForeignKey("SourceClassEntityId")]
        public ClassEntity SourceClassEntity { get; set; }


        public Guid TargetClassEntityId { get; set; }
        [JsonIgnore]
        [ForeignKey("TargetClassEntityId")]
        public ClassEntity TargetClassEntity { get; set; }

        public string RelationshipDescription { get; set; }
        //public string InverseRelationshipDescription { get; set; }
        public bool IsUnique { get; set; } //only allow one relationship from source to target, ie a network switch can reside in only one rack


        public ClassRelationshipEntity()
        {
        }


    }
}
