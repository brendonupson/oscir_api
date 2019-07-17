using System;
using OSCiR.Model;
using Newtonsoft.Json.Linq;
using Application.Interfaces;
using DomainLayer.Exceptions;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using System.Dynamic;
using Newtonsoft.Json;

namespace App
{
    public class ConfigItemManager
    {
        IConfigItemData _configItemRepo;
        BlueprintManager _blueprintManager;

        public ConfigItemManager(IConfigItemData configItemRepo, IBlueprintData blueprintRepo)
        {
            _configItemRepo = configItemRepo;
            _blueprintManager = new BlueprintManager(blueprintRepo, _configItemRepo);
            //Console.WriteLine("MAKING NEW ConfigItemManager()");
        }


        /// <summary>
        /// Loops though properties and remove any that don't appear in the class definition
        /// </summary>
        /// <param name="classEntity">Class entity.</param>
        public static void ProcessProperties(ConfigItemEntity configItem, ClassEntity classEntity)
        {
            if (configItem.Properties == null) return;
            //TODO force lowercase?
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


        public ConfigItemRelationshipEntity CreateConfigItemRelationship(Guid sourceConfigItemId, Guid targetConfigItemId, string relationshipDescription, bool ensureUniqueBetweenClasses, string userName)
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
                //The blueprint has defined a relationship, so use the text defined on that relationship
                relationshipDescription = cr.RelationshipDescription;
                //if (relationshipDescription == null || relationshipDescription.Length == 0) relationshipDescription = cr.RelationshipDescription;
                //if the caller is not specifying uniqueness, check the class relationship setting
                if (!ensureUniqueBetweenClasses) ensureUniqueBetweenClasses = cr.IsUnique;
            }

            //if there is already a relationship, ignore the add
            var existingRelationship = GetConfigItemRelationship(sourceConfigItemId, targetConfigItemId, relationshipDescription);
            if (existingRelationship!=null)
            {
                //Console.WriteLine("**** RELATIONSHIP EXISTS - SKIPPING ADD ****");
                return existingRelationship;
            }

            //remove any 
            if (ensureUniqueBetweenClasses)
            {
                _configItemRepo.DeleteConfigItemRelationshipsToClass(sourceConfigItemId, ciTarget.ClassEntityId, userName);

            }

            if(string.IsNullOrEmpty(relationshipDescription))
            {
                throw new DataWriteException("Cannot add a relationship without a description");
            }

            ConfigItemRelationshipEntity configItemRelationshipEntity = new ConfigItemRelationshipEntity()
            {
                SourceConfigItemEntityId = sourceConfigItemId,
                TargetConfigItemEntityId = targetConfigItemId,
                RelationshipDescription = relationshipDescription,
                CreatedBy = userName,
                ModifiedBy = userName
            };

            return _configItemRepo.CreateConfigItemRelationship(configItemRelationshipEntity);
        }


        public ConfigItemRelationshipEntity GetConfigItemRelationship(Guid sourceConfigItemId, Guid targetConfigItemId, string relationshipDescription)
        {
            var relationships = _configItemRepo.GetConfigItemRelationships(sourceConfigItemId, targetConfigItemId);
            foreach(var relationship in relationships)
            {
                if (relationship.RelationshipDescription.Equals(relationshipDescription)) return relationship;
            }

            return null;
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
            /*ClassEntity ce = _blueprintManager.ReadClass(configItemEntity.ClassEntityId);
            if (!ce.IsInstantiable)
            {
                throw new DataWriteException(ce.ClassName + " may not be instantiated to a ConfigItem");
            }

            //remove any property not in the class schema, or rename to match class
            ConfigItemManager.ProcessProperties(configItemEntity, ce);*/

            configItemEntity = ProcessProperties(configItemEntity);

            return _configItemRepo.UpdateConfigItem(configItemEntity);
        }

        public ConfigItemEntity ProcessProperties(ConfigItemEntity configItemEntity)
        {
            ClassEntity ce = _blueprintManager.ReadClass(configItemEntity.ClassEntityId);
            if (!ce.IsInstantiable)
            {
                throw new DataWriteException(ce.ClassName + " may not be instantiated to a ConfigItem");
            }

            //remove any property not in the class schema, or rename to match class
            ConfigItemManager.ProcessProperties(configItemEntity, ce);
            return configItemEntity;
        }
        

        public static JObject MergeObjects(JObject source, JObject master)
        {
           
            //process properties object first, and store a copy
            var sourceProperties = JObject.FromObject(source["Properties"]);
            if (sourceProperties == null) sourceProperties = new JObject();
            sourceProperties.Merge(master["Properties"], new JsonMergeSettings
            {
                // union array values together to avoid duplicates
                MergeArrayHandling = MergeArrayHandling.Replace, //will blow away properties if only one is specified
                MergeNullValueHandling = MergeNullValueHandling.Merge
            });
            var mergedProperties = sourceProperties;
            //Beware objects nested inside properties! 

            //next process main object
            var sourceConfigItemJObject = JObject.FromObject(source);
            sourceConfigItemJObject.Merge(master, new JsonMergeSettings
            {
                // union array values together to avoid duplicates
                MergeArrayHandling = MergeArrayHandling.Replace, //.Replace will blow away properties if only one is specified
                MergeNullValueHandling = MergeNullValueHandling.Merge
            });

            return sourceConfigItemJObject;
        }


        public ConfigItemEntity PatchConfigItem(Guid configItemId, JObject patchConfigItemJObject)
        {
            var configItem = this.ReadConfigItems(new Guid[] { configItemId }).FirstOrDefault();
            if (configItem == null) throw new DataReadException("ConfigItem Id:" + configItemId + " not found");

            //Make sure property names match, eg "Phone" vs "phone" etc
            var patchConfigItem = patchConfigItemJObject.ToObject<ConfigItemEntity>();
            if (patchConfigItem.ClassEntityId == null || patchConfigItem.ClassEntityId == Guid.Empty) patchConfigItem.ClassEntityId = configItem.ClassEntityId;
            if (patchConfigItem.Properties == null) patchConfigItem.Properties = new JObject();
            patchConfigItem = ProcessProperties(patchConfigItem);
            patchConfigItemJObject.Remove("properties");
            patchConfigItemJObject["Properties"] = patchConfigItem.Properties;

            var source = JObject.FromObject(configItem);
            //configItem = MergeObjects(source, patchConfigItemJObject).ToObject<ConfigItemEntity>();
            //Console.WriteLine(JsonConvert.SerializeObject(patchConfigItemJObject, Formatting.Indented));
            //configItem = MergeObjects(source, JObject.FromObject(patchConfigItem)).ToObject<ConfigItemEntity>();
            configItem = MergeObjects(source, patchConfigItemJObject).ToObject<ConfigItemEntity>();

            //Console.WriteLine(JsonConvert.SerializeObject(configItem, Formatting.Indented));
            /*
            var updatedPatchConfigItemJObject = JObject.FromObject(patchConfigItem);

            patchConfigItemJObject["properties"] = updatedPatchConfigItemJObject["Properties"];

            //process properties object first, and store a copy
            var sourceProperties = JObject.FromObject(configItem.Properties);
            if (sourceProperties == null) sourceProperties = new JObject();
            sourceProperties.Merge(patchConfigItemJObject["properties"], new JsonMergeSettings
            {
                // union array values together to avoid duplicates
                MergeArrayHandling = MergeArrayHandling.Replace, //will blow away properties if only one is specified
                MergeNullValueHandling = MergeNullValueHandling.Merge
            });
            var mergedProperties = sourceProperties;
            //Beware objects nested inside properties! 

            //next process main object
            var sourceConfigItemJObject = JObject.FromObject(configItem);
            sourceConfigItemJObject.Merge(patchConfigItemJObject, new JsonMergeSettings
            {
                // union array values together to avoid duplicates
                MergeArrayHandling = MergeArrayHandling.Replace, //.Replace will blow away properties if only one is specified
                MergeNullValueHandling = MergeNullValueHandling.Merge
            });

            configItem = sourceConfigItemJObject.ToObject<ConfigItemEntity>();
            */
            //add properties object back
            //configItem.Properties = mergedProperties;
            return this.UpdateConfigItem(configItem);
        }

        public static JsonPatchDocument CreatePatch(object originalObject, object modifiedObject)
        {
            var original = JObject.FromObject(originalObject);
            var modified = JObject.FromObject(modifiedObject);

            var patch = new JsonPatchDocument();
            FillPatchForObject(original, modified, patch, "/");

            return patch;
        }

        static void FillPatchForObject(JObject orig, JObject mod, JsonPatchDocument patch, string path)
        {
            var origNames = orig.Properties().Select(x => x.Name).ToArray();
            var modNames = mod.Properties().Select(x => x.Name).ToArray();

            // Names removed in modified
            foreach (var k in origNames.Except(modNames))
            {
                var prop = orig.Property(k);
                patch.Remove(path + prop.Name);
            }

            // Names added in modified
            foreach (var k in modNames.Except(origNames))
            {
                var prop = mod.Property(k);
                patch.Add(path + prop.Name, prop.Value);
            }

            // Present in both
            foreach (var k in origNames.Intersect(modNames))
            {
                var origProp = orig.Property(k);
                var modProp = mod.Property(k);

                if (origProp.Value.Type != modProp.Value.Type)
                {
                    patch.Replace(path + modProp.Name, modProp.Value);
                }
                else if (!string.Equals(
                                origProp.Value.ToString(Newtonsoft.Json.Formatting.None),
                                modProp.Value.ToString(Newtonsoft.Json.Formatting.None)))
                {
                    if (origProp.Value.Type == JTokenType.Object)
                    {
                        // Recurse into objects
                        FillPatchForObject(origProp.Value as JObject, modProp.Value as JObject, patch, path + modProp.Name + "/");
                    }
                    else
                    {
                        // Replace values directly
                        patch.Replace(path + modProp.Name, modProp.Value);
                    }
                }
            }
        }



        public IEnumerable<ConfigItemRelationshipEntity> GetConfigItemRelationships(Guid sourceConfigItemId, Guid? targetConfigItemId)
        {
            return _configItemRepo.GetConfigItemRelationships(sourceConfigItemId, targetConfigItemId);
        }

        public ConfigItemRelationshipEntity ReadConfigItemRelationship(Guid configItemRelationshipGuid)
        {
            return _configItemRepo.ReadConfigItemRelationship(configItemRelationshipGuid);
        }

        public bool DeleteConfigItemRelationship(Guid configItemRelationshipId, string userName)
        {
            return _configItemRepo.DeleteConfigItemRelationship(configItemRelationshipId, userName);
        }


        /*public IEnumerable<ConfigItemEntity> GetConfigItemsForClassOrOwner(Guid? classEntityId, Guid? ownerId, string nameLike, string nameEquals)
        {
            return _configItemRepo.GetConfigItemsForClassOrOwner(classEntityId, ownerId, nameLike, nameEquals);
        }*/

        public IEnumerable<ConfigItemEntity> GetConfigItemsForClassOrOwner(DataSetPager pager, Guid? classEntityId, Guid? ownerId, string nameLike, string nameEquals, string concreteRefEquals)
        {
            return _configItemRepo.GetConfigItemsForClassOrOwner(pager, classEntityId, ownerId, nameLike, nameEquals, concreteRefEquals);
        }

        public IEnumerable<ConfigItemEntity> ReadConfigItems(Guid[] configItemGuids)
        {
            return _configItemRepo.ReadConfigItems(configItemGuids);
        }

        public bool DeleteConfigItem(Guid configItemId, string userName)
        {
            return _configItemRepo.DeleteConfigItem(configItemId, userName);
        }
    }
}