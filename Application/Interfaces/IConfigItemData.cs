using System;
using System.Collections.Generic;
using DomainLayer.Model.AdHoc;
using OSCiR.Model;

namespace Application.Interfaces
{
    public interface IConfigItemData
    {
        ConfigItemEntity CreateConfigItem(ConfigItemEntity configItemEntity);
        ConfigItemEntity UpdateConfigItem(ConfigItemEntity configItemEntity);
        bool DeleteConfigItem(Guid configItemId, string userName);

        IEnumerable<ConfigItemEntity> ReadConfigItems(Guid[] configItemGuids);
        IEnumerable<ConfigItemEntity> GetConfigItemsForClassOrOwner(DataSetPager pager, Guid? classEntityId, Guid? ownerId, string nameLike, string nameEquals, string concreteRefEquals);


        ConfigItemRelationshipEntity CreateConfigItemRelationship(ConfigItemRelationshipEntity configItemRelationshipEntity);
        ConfigItemRelationshipEntity ReadConfigItemRelationship(Guid configItemRelationshipGuid);
        bool DeleteConfigItemRelationship(Guid configItemRelationshipId, string userName);
        IEnumerable<ConfigItemStatistic> GetConfigItemCountsForOwner(Guid ownerEntityId);
        void DeleteConfigItemRelationshipsToClass(Guid sourceConfigItemId, Guid targetClassEntityId, string userName);

        IEnumerable<ConfigItemRelationshipEntity> GetConfigItemRelationships(Guid sourceConfigItemId, Guid? targetConfigItemId);

    }
}
