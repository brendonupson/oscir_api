using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace OSCiR.Model
{
    public class ClassPropertyEntity : BaseEntity
    {
        public string DisplayLabel { get; set; } //sort 3
        public string DisplayGroup { get; set; } //sort level 1
        public int DisplayOrder { get; set; } //sort 2
        public string InternalName { get; set; }
        public string ControlType { get; set; } //text, longtext, computed, list, date, checkbox, toggle
        public string TypeDefinition { get; set; } //list of choices, #FromCIList:, regex
        public string DefaultValue { get; set; }
        public bool IsMandatory { get; set; }
        public string HideWhen { get; set; }

        public string Comments { get; set; }

        [ForeignKey("ClassEntity")]
        public Guid ClassEntityId { get; set; }
        [JsonIgnore]
        public ClassEntity ParentClass { get; set; }

        [NotMapped]
        public const string TYPE_NUMBER = "number";
        [NotMapped]
        public const string TYPE_BOOLEAN = "boolean";
        [NotMapped]
        public const string TYPE_STRING = "string";
        [NotMapped]
        public const string TYPE_DATE = "date";


        public ClassPropertyEntity()
        {
        }

        /*
        internal bool IsNumber()
        {
            return Type != null && Type.ToLower().Equals(TYPE_NUMBER);
        }

        internal bool IsBoolean()
        {
            return Type != null && Type.ToLower().Equals(TYPE_BOOLEAN);
        }

        internal bool IsString()
        {
            return Type != null && Type.ToLower().Equals(TYPE_STRING);
        }

        internal bool IsDate()
        {
            return Type != null && Type.ToLower().Equals(TYPE_DATE);
        }*/
    }
}
