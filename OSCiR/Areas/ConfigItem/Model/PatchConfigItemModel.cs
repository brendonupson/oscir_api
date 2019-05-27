using System;
using Newtonsoft.Json.Linq;
using OSCiR.Model;

namespace OSCiR.Areas.ConfigItem.Model
{
    public class PatchConfigItemModel
    {
        public Guid[] ConfigItemIds { get; set; }
        public JObject patchConfigItem { get; set; } //may contain some or all fields of ConfigItem
    }
}
