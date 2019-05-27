using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace OSCiR.Model
{
    public class BaseEntity
    {
        [NotMapped]
        [JsonIgnore]
        public const string ANONYMOUS_USER = "Anonymous";


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }

        //These will be null unless the record is actally deleted
        //This provides a "soft delete" capability
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }

        public BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.Now;
            ModifiedOn = DateTime.Now;
        }
    }
}
