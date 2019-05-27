using System;
using System.Collections.Generic;
using System.Linq;
using OSCiR.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Application.Interfaces;
using DomainLayer.Exceptions;

namespace OSCiR.Datastore
{
    public class BlueprintRepository : IBlueprintData
    {
        private DbContext _dbContext;
        private DbSet<ClassEntity> _classSet { get; set; }
        private DbSet<ClassPropertyEntity> _classPropertySet { get; set; }
        private DbSet<ClassExtendEntity> _classExtendSet { get; set; }
        private DbSet<ClassRelationshipEntity> _classRelationshipSet { get; set; }


        public BlueprintRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _classSet = _dbContext.Set<ClassEntity>();
            _classPropertySet = _dbContext.Set<ClassPropertyEntity>();
            _classExtendSet = _dbContext.Set<ClassExtendEntity>();
            _classRelationshipSet = _dbContext.Set<ClassRelationshipEntity>();
        }

        public IEnumerable<ClassEntity> ReadClasses(Guid[] classGuids)
        {
            try
            {
                IEnumerable<ClassEntity> ce = _classSet.AsNoTracking().Where(p => classGuids.Contains(p.Id))
                    .Include(i => i.Properties)
                    .Include(i => i.Extends)
                    .Include(i => i.SourceRelationships)
                    .Include(i => i.TargetRelationships)
                    .AsEnumerable<ClassEntity>();

                return ce;
            }
            catch (Exception e)
            {
                throw new DataReadException(e.Message, e);
            }

        }

        public bool DeleteClass(Guid classEntityId, string userName)
        {
            try
            {
                var cList = ReadClasses(new Guid[] { classEntityId });
                if (cList.Count() == 1)
                {
                    var c = cList.First();
                    c.DeletedOn = DateTime.Now;
                    c.DeletedBy = userName;
                    UpdateClass(c);
                    return true;
                }
                /*ClassEntity classToBeDeleted = new ClassEntity { Id = id };
                _dbContext.Entry(classToBeDeleted).State = EntityState.Deleted;
                _dbContext.SaveChanges();*/
                return false;
            }
            catch (Exception e)
            {
                throw new DataReadException(e.Message, e);
            }
        }

        public IEnumerable<ClassEntity> ReadClasses(string classNameContains, string classNameEquals, string classCategoryEquals)
        {

            try
            {
                var classResults = _classSet.Where(a => ((String.IsNullOrEmpty(classNameContains) || a.ClassName.ToLower().Contains(classNameContains.ToLower())) &&
                                                            (String.IsNullOrEmpty(classNameEquals) || a.ClassName.ToLower().Equals(classNameEquals.ToLower())) &&
                                                            (String.IsNullOrEmpty(classCategoryEquals) || a.Category.ToLower().Equals(classCategoryEquals.ToLower()))))
                            .OrderBy(o => o.ClassName)
                            .AsNoTracking().ToList<ClassEntity>();
                return classResults;
            }
            catch (Exception e)
            {
                throw new DataReadException(e.Message, e);
            }
        }

        public ClassEntity CreateClass(ClassEntity classEntity)
        {

            try
            {
                _classSet.Add(classEntity);
                _dbContext.SaveChanges();

                return classEntity;
            }
            catch (Exception e)
            {
                throw new DataWriteException("CreateClass(): " + e.Message, e);
            }
        }

        public ClassEntity UpdateClass(ClassEntity classEntity)
        {

            try
            {
                classEntity.ModifiedOn = DateTime.Now;

                _dbContext.Entry(classEntity).State = EntityState.Modified;
                _dbContext.Entry(classEntity).Property(x => x.CreatedBy).IsModified = false;
                _dbContext.Entry(classEntity).Property(x => x.CreatedOn).IsModified = false;
                _dbContext.Entry(classEntity).Property(x => x.Id).IsModified = false;
                //_dbContext.Entry(classEntity).Property(x => x.Properties).IsModified = false;
                //_dbContext.Entry(classEntity).Property(x => x.Extends).IsModified = false;
                //_dbContext.Entry(classEntity).Property(x => x.Relationships).IsModified = false;
                _dbContext.SaveChanges();
                _dbContext.Entry(classEntity).Reload();
                return classEntity;
            }
            catch (Exception e)
            {
                throw new DataWriteException("UpdateClass(): " + e.Message, e);
            }
        }





        // ====== PROPERTIES



        public ClassPropertyEntity CreateClassProperty(ClassPropertyEntity propertyEntity)
        {

            try
            {
                _classPropertySet.Add(propertyEntity);
                _dbContext.SaveChanges();

                return propertyEntity;
            }
            catch (Exception e)
            {
                throw new DataWriteException("CreateClassProperty(): " + e.Message, e);
            }
        }

        public ClassPropertyEntity UpdateClassProperty(ClassPropertyEntity propertyEntity)
        {

            try
            {
                propertyEntity.ModifiedOn = DateTime.Now;

                _dbContext.Entry(propertyEntity).State = EntityState.Modified;
                _dbContext.Entry(propertyEntity).Property(x => x.CreatedBy).IsModified = false;
                _dbContext.Entry(propertyEntity).Property(x => x.CreatedOn).IsModified = false;
                _dbContext.Entry(propertyEntity).Property(x => x.Id).IsModified = false;

                _dbContext.SaveChanges();
                _dbContext.Entry(propertyEntity).Reload();
                return propertyEntity;
            }
            catch (Exception e)
            {
                throw new DataWriteException("UpdateClassProperty(): " + e.Message, e);
            }
        }

        public bool DeleteClassProperty(Guid classPropertyGuid)
        {

            try
            {
                ClassPropertyEntity classToBeDeleted = new ClassPropertyEntity { Id = classPropertyGuid };
                _dbContext.Entry(classToBeDeleted).State = EntityState.Deleted;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new DataWriteException("CreateClassProperty(): " + e.Message, e);
            }
        }


        // ====== ClassExtend

        public ClassExtendEntity CreateClassExtend(ClassExtendEntity extendEntity)
        {

            try
            {
                _classExtendSet.Add(extendEntity);
                _dbContext.SaveChanges();

                return extendEntity;
            }
            catch (Exception e)
            {
                throw new DataWriteException("CreateClassExtend(): " + e.Message, e);
            }
        }

        public bool DeleteClassExtend(Guid classExtendGuid)
        {
            try
            {
                ClassExtendEntity classToBeDeleted = new ClassExtendEntity { Id = classExtendGuid };
                _dbContext.Entry(classToBeDeleted).State = EntityState.Deleted;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new DataWriteException("DeleteClassExtend(): " + e.Message, e);
            }

        }


        // ========== ClassRelationship

        public ClassRelationshipEntity ReadClassRelationship(Guid classRelationshipId)
        {
            if (classRelationshipId == null || classRelationshipId == Guid.Empty) throw new DataReadException("Invalid classRelationshipId passed");

            try
            {
                ClassRelationshipEntity cr = _classRelationshipSet.AsNoTracking().Where(p => p.Id == classRelationshipId)
                    .FirstOrDefault();

                return cr;
            }
            catch (Exception e)
            {
                throw new DataReadException("ReadClassRelationship(): " + e.Message, e);
            }
        }


        // ========== Class Relationships

        public ClassRelationshipEntity CreateClassRelationship(ClassRelationshipEntity relationshipEntity)
        {

            try
            {
                _classRelationshipSet.Add(relationshipEntity);
                _dbContext.SaveChanges();

                return relationshipEntity;
            }
            catch (Exception e)
            {
                throw new DataWriteException("InsertClassRelationship(): " + e.Message, e);
            }
        }

        public ClassRelationshipEntity GetClassRelationship(Guid sourceClassEntityId, Guid targetClassEntityId, string relationshipHint)
        {
            //FIXME this should be in app layer

            //get all class relationships between classes both ways
            var relationships = _classRelationshipSet.Where(a => (a.SourceClassEntityId == sourceClassEntityId && a.TargetClassEntityId == targetClassEntityId) ||
            (a.SourceClassEntityId == targetClassEntityId && a.TargetClassEntityId == sourceClassEntityId))
                            .OrderBy(o => o.SourceClassEntityId)
                            .AsNoTracking().ToList<ClassRelationshipEntity>();
            if (relationships.Count == 0) return null;

            //try to find a matching relationship by hint (eg "Depends On")
            if (relationshipHint != null && relationshipHint.Length > 0)
            {
                foreach (var rel in relationships)
                {
                    if (rel.RelationshipDescription.ToLower().Equals(relationshipHint.ToLower())) return rel;
                }
            }

            //if not found above, just return the first whatever that may be
            return relationships.First<ClassRelationshipEntity>();
        }

        public ClassEntity GetClassFullPropertyDefinition(Guid classGuid)
        {

            if (classGuid == Guid.Empty) return null;

            try
            {
                ClassEntity ce = _classSet.AsNoTracking().Where(p => p.Id == classGuid)
                    .Include(i => i.Properties)
                    .Include(i => i.Extends)
                    .FirstOrDefault();

                List<ClassPropertyEntity> allProperties = new List<ClassPropertyEntity>();
                foreach (var ent in ce.Extends)
                {
                    ClassEntity ce2 = _classSet.AsNoTracking().Where(p => p.Id == ent.ExtendsClassEntityId)
                    .Include(i => i.Properties)
                    //.Include(i => i.Extends)
                    .FirstOrDefault();

                    foreach (var prop in ce2.Properties)
                    {
                        allProperties.Add(prop);
                    }
                }

                foreach (var prop in ce.Properties)
                {
                    allProperties.Add(prop);
                }

                var sortedProperties = allProperties.OrderBy(x => x.DisplayGroup)
                    .ThenBy(x => x.DisplayOrder)
                    .ThenBy(x => x.DisplayLabel);


                List<ClassPropertyEntity> finalProperties = new List<ClassPropertyEntity>();
                foreach (var prop in sortedProperties)
                {
                    finalProperties.Add(prop);
                }

                //make an entity to return
                ClassEntity ceReply = _classSet.AsNoTracking().Where(p => p.Id == classGuid)
                    .Include(i => i.Extends)
                    .Include(i => i.SourceRelationships)
                    .Include(i => i.TargetRelationships)
                    .FirstOrDefault();
                ceReply.Properties = finalProperties;
                return ceReply;
            }
            catch (Exception e)
            {
                throw new DataWriteException("GetClassFullPropertyDefinition(): " + e.Message, e);
            }

        }

        public bool DeleteClassRelationship(Guid classRelationshipGuid, string userName)
        {

            try
            {
                //ClassRelationshipEntity classToBeDeleted = new ClassRelationshipEntity { Id = classRelationshipGuid };
                //_dbContext.Entry(classToBeDeleted).State = EntityState.Deleted;
                //_dbContext.SaveChanges();
                ClassRelationshipEntity cir = _classRelationshipSet.Where(p => p.Id == classRelationshipGuid).FirstOrDefault();
                _dbContext.Entry(cir).State = EntityState.Modified;
                cir.DeletedOn = DateTime.Now;
                cir.DeletedBy = userName;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new DataWriteException("DeleteClassRelationship(): " + e.Message, e);
            }
        }

    }
}
