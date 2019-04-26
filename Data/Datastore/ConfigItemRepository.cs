using System;
using System.Collections.Generic;
using System.Linq;
using App;
using OSCiR.Model;
using Microsoft.EntityFrameworkCore;
using DomainLayer.Exceptions;
using Application.Interfaces;

namespace OSCiR.Areas.Admin.Class.Model
{
    public class ConfigItemRepository : IConfigItemData
    {
        private DbContext _dbContext;
        private DbSet<ConfigItemEntity> _ciSet { get; set; }
        private DbSet<ConfigItemRelationshipEntity> _cirSet { get; set; }


        public ConfigItemRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _ciSet = _dbContext.Set<ConfigItemEntity>();
            _cirSet = _dbContext.Set<ConfigItemRelationshipEntity>();
        }


        public IEnumerable<ConfigItemEntity> ReadConfigItems(Guid[] configItemGuids)
        {
            if (configItemGuids == null) return null;

            try
            {
                IEnumerable <ConfigItemEntity> cis = _ciSet.AsNoTracking().Where(p => configItemGuids.Contains(p.Id))
                    .Include(i => i.SourceRelationships)
                    .Include(i => i.TargetRelationships)
                    .AsEnumerable<ConfigItemEntity>();

                return cis;
            }
            catch (Exception e)
            {
                throw new DataReadException(e.Message, e);
            }
        }



        public bool DeleteConfigItem(Guid configItemId)
        {

            try
            {
                ConfigItemEntity ciToBeDeleted = new ConfigItemEntity { Id = configItemId };
                _dbContext.Entry(ciToBeDeleted).State = EntityState.Deleted;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new DataReadException(e.Message, e);
            }


        }


        public IEnumerable<ConfigItemEntity> GetConfigItemsForClassOrOwner(Guid? classEntityId, Guid? ownerId)
        {
            if (classEntityId == null && ownerId == null) throw new DataReadException("classEntityId or ownerId must be specified");

            try
            {
                var result = _ciSet.Where(a => (classEntityId==null?true:a.ClassEntityId == classEntityId) &&
                            (ownerId==null?true:a.OwnerId == ownerId) )
                            .Include(i => i.SourceRelationships)
                            .Include(i => i.TargetRelationships)
                            .OrderBy(o => o.Name)
                            .AsNoTracking().ToList<ConfigItemEntity>();
                return result;
            }
            catch (Exception e)
            {
                throw new DataReadException(e.Message, e);
            }
        }


        public ConfigItemEntity CreateConfigItem(ConfigItemEntity configItemEntity)
        {

            try
            {
                _ciSet.Add(configItemEntity);
                _dbContext.SaveChanges();

                return configItemEntity;
            }
            catch (Exception e)
            {
                throw new DataWriteException("CreateConfigItem(): " + e.Message, e);
            }
        }


        public ConfigItemEntity UpdateConfigItem(ConfigItemEntity configItemEntity)
        {

            try
            {
                _dbContext.Entry(configItemEntity).State = EntityState.Modified;

                configItemEntity.ModifiedOn = DateTime.Now;
                _dbContext.Entry(configItemEntity).Property(x => x.CreatedBy).IsModified = false;
                _dbContext.Entry(configItemEntity).Property(x => x.CreatedOn).IsModified = false;
                _dbContext.Entry(configItemEntity).Property(x => x.Id).IsModified = false;

                _dbContext.SaveChanges();
                _dbContext.Entry(configItemEntity).Reload();

                return configItemEntity;
            }
            catch (Exception e)
            {
                throw new DataWriteException("UpdateConfigItem(): " + e.Message, e);
            }
        }

        //======= Relationship

        public ConfigItemRelationshipEntity CreateConfigItemRelationship(ConfigItemRelationshipEntity configItemRelationshipEntity)
        {
            try 
            { 
            _cirSet.Add(configItemRelationshipEntity);
            _dbContext.SaveChanges();

            return configItemRelationshipEntity;
        }
            catch (Exception e)
            {
                throw new DataWriteException("CreateConfigItemRelationship(): " + e.Message, e);
            }
}

            

        public IEnumerable<ConfigItemRelationshipEntity> GetConfigItemRelationships(Guid sourceConfigItemId, Guid? targetConfigItemId)
        {
            if (sourceConfigItemId == Guid.Empty)
            {
                throw new DataReadException("Source Config Item is empty");
            }

            try
            {
                if (targetConfigItemId != null && targetConfigItemId != Guid.Empty)
                {
                    var ciRelations = _cirSet.AsNoTracking().Where(p => p.SourceConfigItemEntityId == sourceConfigItemId &&
                    p.TargetConfigItemEntityId == targetConfigItemId).ToList();
                    return ciRelations;
                }

                var cir = _cirSet.AsNoTracking().Where(p => p.SourceConfigItemEntityId == sourceConfigItemId).ToList();
                return cir;
            }
            catch (Exception e)
            {
                throw new DataReadException(e.Message, e);
            }
        }

        public ConfigItemRelationshipEntity ReadConfigItemRelationship(Guid configItemRelationshipGuid)
        {

            if (configItemRelationshipGuid == Guid.Empty) throw new DataReadException("configItemRelationshipGuid may not be empty");

            try
            {
                ConfigItemRelationshipEntity cir = _cirSet.AsNoTracking().Where(p => p.Id == configItemRelationshipGuid)
                    .FirstOrDefault();

                return cir;
            }
            catch (Exception e)
            {
                throw new DataReadException(e.Message, e);
            }
        }


        public bool DeleteConfigItemRelationship(Guid configItemRelationshipId)
        {
            try
            {
                ConfigItemRelationshipEntity ciToBeDeleted = new ConfigItemRelationshipEntity { Id = configItemRelationshipId };
                //_dbContext.Entry(ciToBeDeleted).State = EntityState.Deleted;
                _dbContext.Remove(ciToBeDeleted);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new DataWriteException(e.Message, e);
            }


        }


    }
}
