
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OSCiR.Model
{
    public class OwnerEntity : BaseEntity
    {
        public string OwnerName { get; set; }
        public string OwnerCode { get; set; }
        public string AlternateName1 { get; set; }
        public string Category { get; set; }
        public string Comments { get; set; }
        public string Status { get; set;  } //active, inactive, ...

        [JsonIgnore]
        public List<ConfigItemEntity> ConfigItems { get; set; }

        public OwnerEntity() : base()
        {

        }
    }
}
