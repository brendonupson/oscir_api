using System;
using System.Collections.Generic;
using Application.Interfaces;
using DomainLayer.Model.AdHoc;

namespace ApplicationLayer.App
{
    public class ReportManager
    {
        IConfigItemData _configItemRepo;
        IOwnerData _ownerRepo;


        public ReportManager(IConfigItemData configItemRepo, IOwnerData ownerRepo)
        {
            _configItemRepo = configItemRepo;
            _ownerRepo = ownerRepo;
        }


        public IEnumerable<OwnerStatistic> GetOwnerStatistics(Guid? ownerEntityId)
        {
            List<OwnerStatistic> stats = new List<OwnerStatistic>(100);
            var owners = _ownerRepo.GetOwners(ownerEntityId);

            foreach (var owner in owners)
            {
                OwnerStatistic os = new OwnerStatistic() { OwnerEntityId = owner.Id, OwnerName = owner.OwnerName };
                os.ConfigItemStatistics = _configItemRepo.GetConfigItemCountsForOwner(owner.Id);

                stats.Add(os);
            }

            return stats;
        }

        public IEnumerable<ConfigItemStatistic> GetClassStatistics(Guid? classEntityId)
        {            
            var stats = _configItemRepo.GetConfigItemCountsForClass(classEntityId);            
            return stats;
        }
    }
}
