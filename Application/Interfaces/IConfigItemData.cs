using System;
using System.Collections.Generic;
using OSCiR.Model;

namespace Application.Interfaces
{
    public interface IConfigItemData
    {
        ConfigItemEntity CreateConfigItem(ConfigItemEntity configItemEntity);
        ConfigItemEntity UpdateConfigItem(ConfigItemEntity configItemEntity);
        bool DeleteConfigItem(Guid configItemId);

        IEnumerable<ConfigItemEntity> ReadConfigItems(Guid[] configItemGuids);
        IEnumerable<ConfigItemEntity> GetConfigItemsForClassOrOwner(Guid? classEntityId, Guid? ownerId);


        ConfigItemRelationshipEntity CreateConfigItemRelationship(ConfigItemRelationshipEntity configItemRelationshipEntity);
        ConfigItemRelationshipEntity ReadConfigItemRelationship(Guid configItemRelationshipGuid);
        bool DeleteConfigItemRelationship(Guid configItemRelationshipId);

        IEnumerable<ConfigItemRelationshipEntity> GetConfigItemRelationships(Guid sourceConfigItemId, Guid? targetConfigItemId);

    }
}
