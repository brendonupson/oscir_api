using System;
using OSCiR.Model;
using Newtonsoft.Json.Linq;
using Application.Interfaces;
using DomainLayer.Exceptions;
using System.Linq;
using System.Collections.Generic;

namespace App
{
    public class ConfigItemManager
    {
        IConfigItemData _configItemRepo;
        BlueprintManager _blueprintManager;

        public ConfigItemManager(IConfigItemData configItemRepo, IBlueprintData blueprintRepo)
        {
            _configItemRepo = configItemRepo;
            _blueprintManager = new BlueprintManager(blueprintRepo);
        }


        /// <summary>
        /// Loops though properties and remove any that don't appear in the class definition
        /// </summary>
        /// <param name="classEntity">Class entity.</param>
        public static void ProcessProperties(ConfigItemEntity configItem, ClassEntity classEntity)
        {
            foreach (var prop in new JObject(configItem.Properties))
            {
                var classProperty = classEntity.GetProperty(prop.Key);
                if (classProperty != null) //if has the named property
                {
                    // set the key as per the class eg yourName vs yourname vs YourNAME
                    JProperty localProp = configItem.Properties.Property(prop.Key);
                    localProp.Value.Rename(classProperty.InternalName);
                }
                else
                {
                    if (!classEntity.AllowAnyData) configItem.Properties.Remove(prop.Key);
                }
            }
        }


        public ConfigItemRelationshipEntity CreateConfigItemRelationship(Guid sourceConfigItemId, Guid targetConfigItemId, string relationshipDescription, string userName)
        {


            if (sourceConfigItemId == Guid.Empty || targetConfigItemId == Guid.Empty)
            {
                throw new DataWriteException("CreateConfigItemRelationship(): Check source and target references");
            }

            if (sourceConfigItemId == targetConfigItemId)
            {
                throw new DataWriteException("CreateConfigItemRelationship(): Source and target references must be different");
            }

            ConfigItemEntity ciSource = _configItemRepo.ReadConfigItems(new Guid[] { sourceConfigItemId }).FirstOrDefault();
            if (ciSource == null)
            {
                throw new DataWriteException("CreateConfigItemRelationship(): Source CI not found");
            }
            ConfigItemEntity ciTarget = _configItemRepo.ReadConfigItems(new Guid[] { targetConfigItemId }).FirstOrDefault();
            if (ciTarget == null)
            {
                throw new DataWriteException("CreateConfigItemRelationship(): Target CI not found");
            }

            //string inverseRelationshipDescription = null;
            ClassRelationshipEntity cr = _blueprintManager.GetClassRelationship(ciSource.ClassEntityId, ciTarget.ClassEntityId, relationshipDescription);
            if (cr == null)
            {
                ClassEntity sourceClass = _blueprintManager.ReadClasses(new Guid[] { ciSource.ClassEntityId }).FirstOrDefault();
                ClassEntity targetClass = _blueprintManager.ReadClasses(new Guid[] { ciTarget.ClassEntityId }).FirstOrDefault();

                //if one of the two is promiscuous, allow the relationship
                if (sourceClass == null || targetClass == null || !(sourceClass.IsPromiscuous || targetClass.IsPromiscuous))
                {
                    var msg = string.Format("Class relationship could not be found or is not allowed between these classes: \"{0}\" and \"{1}\". Either define a blueprint relationship, or mark at least one promiscuous", sourceClass == null ? "?" : sourceClass.ClassName, targetClass == null ? "?" : targetClass.ClassName);
                    throw new DataWriteException("CreateConfigItemRelationship(): " + msg);
                }
            }
            else
            {
                if (relationshipDescription == null || relationshipDescription.Length == 0) relationshipDescription = cr.RelationshipDescription;
                //inverseRelationshipDescription = cr.InverseRelationshipDescription;
            }


            ConfigItemRelationshipEntity configItemRelationshipEntity = new ConfigItemRelationshipEntity()
            {
                SourceConfigItemEntityId = sourceConfigItemId,
                TargetConfigItemEntityId = targetConfigItemId,
                //ClassRelationshipEntityId = cr.Id,
                RelationshipDescription = relationshipDescription,
                CreatedBy = userName,
                ModifiedBy = userName
            };

            return _configItemRepo.CreateConfigItemRelationship(configItemRelationshipEntity);
        }

        public ConfigItemEntity CreateConfigItem(ConfigItemEntity configItemEntity)
        {
            if (configItemEntity.OwnerId == null || configItemEntity.OwnerId == Guid.Empty)
            {
                throw new DataWriteException("Owner not specified");
            }

            if (configItemEntity.Name == null || configItemEntity.Name == "")
            {
                throw new DataWriteException("Name not specified");
            }


            ClassEntity ce = _blueprintManager.GetClassFullPropertyDefinition(configItemEntity.ClassEntityId);
            if (!ce.IsInstantiable)
            {
                throw new DataWriteException(ce.ClassName + " may not be instantiated to a ConfigItem");
            }

            //remove any property not in the class schema, or rename to match class
            ConfigItemManager.ProcessProperties(configItemEntity, ce);

            return _configItemRepo.CreateConfigItem(configItemEntity);
        }

        public ConfigItemEntity UpdateConfigItem(ConfigItemEntity configItemEntity)
        {
            ClassEntity ce = _blueprintManager.ReadClass(configItemEntity.ClassEntityId);
            if (!ce.IsInstantiable)
            {
                throw new DataWriteException(ce.ClassName + " may not be instantiated to a ConfigItem");
            }

            //remove any property not in the class schema, or rename to match class
            ConfigItemManager.ProcessProperties(configItemEntity, ce);

            return _configItemRepo.UpdateConfigItem(configItemEntity);
        }

        public IEnumerable<ConfigItemRelationshipEntity> GetConfigItemRelationships(Guid sourceConfigItemId, Guid? targetConfigItemId)
        {
            return _configItemRepo.GetConfigItemRelationships(sourceConfigItemId, targetConfigItemId);
        }

        public ConfigItemRelationshipEntity ReadConfigItemRelationship(Guid configItemRelationshipGuid)
        {
            return _configItemRepo.ReadConfigItemRelationship(configItemRelationshipGuid);
        }

        public bool DeleteConfigItemRelationship(Guid configItemRelationshipId)
        {
            return _configItemRepo.DeleteConfigItemRelationship(configItemRelationshipId);
        }

        public IEnumerable<ConfigItemEntity> GetConfigItemsForClassOrOwner(Guid? classEntityId, Guid? ownerId)
        {
            return _configItemRepo.GetConfigItemsForClassOrOwner(classEntityId, ownerId);
        }

        public IEnumerable<ConfigItemEntity> ReadConfigItems(Guid[] configItemGuids)
        {
            return _configItemRepo.ReadConfigItems(configItemGuids);
        }

        public bool DeleteConfigItem(Guid configItemId)
        {
            return _configItemRepo.DeleteConfigItem(configItemId);
        }
    }
}