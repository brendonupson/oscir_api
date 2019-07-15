using System;
using Application.Interfaces;
using OSCiR.Model;
using DomainLayer.Exceptions;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace App
{
    public class BlueprintManager
    {
        IBlueprintData _blueprintRepo;
        IConfigItemData _configItemRepo;

        public BlueprintManager(IBlueprintData blueprintRepo, IConfigItemData configItemRepo)
        {
            _blueprintRepo = blueprintRepo;
            _configItemRepo = configItemRepo;
    }

        public ClassEntity CreateClass(ClassEntity classEntity)
        {
            return _blueprintRepo.CreateClass(classEntity);
        }

        public ClassEntity ReadClass(Guid classGuid)
        {
            return _blueprintRepo.ReadClasses(new Guid[] { classGuid }).FirstOrDefault();
        }

        public IEnumerable<ClassEntity> ReadClasses(Guid[] classGuids)
        {
            return _blueprintRepo.ReadClasses(classGuids);
        }

        public ClassEntity UpdateClass(ClassEntity classEntity)
        {
            return _blueprintRepo.UpdateClass(classEntity);
        }

        public bool DeleteClass(Guid classGuid, string userName)
        {
            DataSetPager pager = new DataSetPager();
            var configItemList = _configItemRepo.GetConfigItemsForClassOrOwner(pager, classGuid, null, null, null, null);
            if(configItemList.Count()>0)
            {
                throw new DataWriteException("Cannot delete Class while ConfigItems using this class exist");
            }

            var classEntity = _blueprintRepo.ReadClasses(new Guid[] { classGuid }).FirstOrDefault();                
            if (classEntity!=null && _blueprintRepo.DeleteClass(classGuid, userName))
            {
                foreach(var rel in classEntity.SourceRelationships)
                {
                    _blueprintRepo.DeleteClassRelationship(rel.Id, userName);
                }
                foreach (var rel in classEntity.TargetRelationships)
                {
                    _blueprintRepo.DeleteClassRelationship(rel.Id, userName);
                }
                return true;
            }
            return false;
        }

        public IEnumerable<ClassEntity> ReadClasses(string classNameContains, string classNameEquals, string classCategoryEquals, bool getUsedClassesOnly)
        {
            return _blueprintRepo.ReadClasses(classNameContains, classNameEquals, classCategoryEquals, getUsedClassesOnly);
        }

        public ClassRelationshipEntity CreateClassRelationship(ClassRelationshipEntity relationshipEntity)
        {
            if (relationshipEntity.SourceClassEntityId == Guid.Empty || relationshipEntity.TargetClassEntityId == Guid.Empty)
            {
                throw new DataWriteException("Check source and target references");
            }

            //allow relationships to same Class. eg a "PERSON" class can be related to another (parent/child, boss/subordinate, etc) 
            var sourceClass = _blueprintRepo.ReadClasses(new Guid[] { relationshipEntity.SourceClassEntityId }).FirstOrDefault();
            if (sourceClass == null || !sourceClass.IsInstantiable)
            {
                throw new DataWriteException("Source class [" + sourceClass.ClassName + "] not found or is not instantiable");
            }

            var targetClass = _blueprintRepo.ReadClasses(new Guid[] { relationshipEntity.TargetClassEntityId }).FirstOrDefault();
            if (targetClass == null || !targetClass.IsInstantiable)
            {
                throw new DataWriteException("Target class [" + targetClass.ClassName + "] not found or is not instantiable");
            }

            return _blueprintRepo.CreateClassRelationship(relationshipEntity);

        }

        public bool DeleteClassExtend(Guid classExtendGuid)
        {
            return _blueprintRepo.DeleteClassExtend(classExtendGuid);
        }

        public ClassExtendEntity CreateClassExtend(ClassExtendEntity classExtendEntity)
        {
            return _blueprintRepo.CreateClassExtend(classExtendEntity);
        }

        public ClassRelationshipEntity ReadClassRelationship(Guid classRelationshipGuid)
        {
            return _blueprintRepo.ReadClassRelationship(classRelationshipGuid);
        }

        public ClassRelationshipEntity GetClassRelationship(Guid sourceClassEntityId, Guid targetClassEntityId, string relationshipDescription)
        {
            return _blueprintRepo.GetClassRelationship(sourceClassEntityId, targetClassEntityId, relationshipDescription);
        }


        public ClassPropertyEntity UpdateClassProperty(ClassPropertyEntity classPropertyEntity)
        {
            return _blueprintRepo.UpdateClassProperty(classPropertyEntity);
        }

        public ClassPropertyEntity CreateClassProperty(ClassPropertyEntity classPropertyEntity)
        {
            return _blueprintRepo.CreateClassProperty(classPropertyEntity);
        }

        public bool DeleteClassRelationship(Guid classRelationshipGuid, string userName)
        {
            return _blueprintRepo.DeleteClassRelationship(classRelationshipGuid, userName);
        }

        public ClassEntity GetClassFullPropertyDefinition(Guid classEntityId)
        {
            return _blueprintRepo.GetClassFullPropertyDefinition(classEntityId);
        }

        public ConfigItemEntity GetConfigItemExample(Guid classEntityGuid)
        {
            if (classEntityGuid==Guid.Empty)
            {
                throw new DataReadException("Invalid classEntityGuid passed");
            }

            try
            {

                ClassEntity ce = _blueprintRepo.GetClassFullPropertyDefinition(classEntityGuid);

                var ci = new ConfigItemEntity();
                ci.Name = "Your CI Name";
                ci.Comments = "string";
                ci.ConcreteReference = "string";

                var obj = new JObject();
                foreach (var prop in ce.Properties)
                {
                    var name = prop.InternalName;
                    if (prop.ControlType == "text")
                    {
                        obj.Add(name, "string value");
                    }
                    if (prop.ControlType == "longtext")
                    {
                        obj.Add(name, "a long text paragraph");
                    }
                    if (prop.ControlType == "toggle")
                    {
                        obj.Add(name, "true");
                    }

                    if (prop.ControlType == "date")
                    {
                        obj.Add(name, DateTime.UtcNow.ToString("o"));
                    }
                }

                ci.Properties = obj;
                return ci;
            }
            catch (Exception e)
            {
                throw new DataReadException("GetConfigItemExample(): " + e.ToString(), e);
            }
        }

        public bool DeleteClassProperty(Guid classPropertyGuid)
        {
            return _blueprintRepo.DeleteClassProperty(classPropertyGuid);
        }
    }
}
