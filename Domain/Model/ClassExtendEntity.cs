using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace OSCiR.Model
{
    public class ClassExtendEntity : BaseEntity
    {
        public Guid ExtendsClassEntityId { get; set; }
        [JsonIgnore]
        [ForeignKey("ExtendsClassEntityId")]
        public ClassEntity ExtendsClass { get; set; }
        //[JsonIgnore]
        //[ForeignKey("ExtendsClassEntityId")]
        //public ClassEntity ExtendsClass { get; set; }


        public Guid ClassEntityId { get; set; }
        [JsonIgnore]
        [ForeignKey("ClassEntityId")]
        public ClassEntity ParentClass { get; set; }


    }
}
