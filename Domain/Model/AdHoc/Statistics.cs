﻿using System;
using System.Collections.Generic;

namespace DomainLayer.Model.AdHoc
{
    public class OwnerStatistic
    {
        public Guid OwnerEntityId { get; set; }
        public string OwnerName { get; set; }
        public IEnumerable<ConfigItemStatistic> ConfigItemStatistics { get; set; }
    }

    public class ConfigItemStatistic
    {
        public Guid ClassEntityId { get; set; }
        public string ClassName { get; set; }
        public string ColorCode { get; set; }
        public int Count { get; set; }
    }

    public class DayStatistic
    {
        public DateTime day { get; set; }        
        public IEnumerable<ConfigItemStatistic> ConfigItemStatistics { get; set; }
    }

    //TODO Category statistics, eg all "Network" devices?
}
