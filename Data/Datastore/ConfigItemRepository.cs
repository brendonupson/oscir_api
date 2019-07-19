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
// https://docs.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlockslim?redirectedfrom=MSDN&view=netframework-4.8
//TODO ??
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
                   // && p.DeletedOn == null)
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



        public bool DeleteConfigItem(Guid configItemId, string userName)
        {

            try
            {
                //ConfigItemEntity ciToBeDeleted = new ConfigItemEntity { Id = configItemId };
                var ciList = ReadConfigItems(new Guid[] { configItemId });
                if(ciList.Count()==1)
                {
                    var ci = ciList.First();
                    //do a copy first so we don't get any data concurrency errors
                    List<ConfigItemRelationshipEntity> relationshipList = new List<ConfigItemRelationshipEntity>();
                    foreach(var rel in ci.SourceRelationships)
                    {
                        relationshipList.Add(rel);
                    }
                    foreach (var rel in ci.TargetRelationships)
                    {
                        relationshipList.Add(rel);
                    }

                    ci.DeletedOn = DateTime.Now;
                    ci.DeletedBy = userName;
                    UpdateConfigItem(ci);
                    //Now remove any relationships to or from this ci
                    DeleteConfigItemRelationships(relationshipList, userName);                    
                    return true;
                }
                //_dbContext.Entry(ciToBeDeleted).State = EntityState.Deleted;
                //_dbContext.SaveChanges();
                return false;
            }
            catch (Exception e)
            {
                throw new DataReadException(e.Message, e);
            }


        }

        public IEnumerable<ConfigItemEntity> GetConfigItemsForClassOrOwner(DataSetPager pager, Guid? classEntityId, Guid? ownerId, string nameLike, string nameEquals, string concreteRefEquals)
        {

            if (classEntityId == null && ownerId == null && string.IsNullOrEmpty(nameLike) && string.IsNullOrEmpty(nameEquals) && string.IsNullOrEmpty(concreteRefEquals) ) throw new DataReadException("At least one paramete must be specified");
            if (pager == null) pager = new DataSetPager();

            bool skipNameLike = String.IsNullOrEmpty(nameLike);
            bool skipNameEquals = String.IsNullOrEmpty(nameEquals);
            bool skipConcreteRef = String.IsNullOrEmpty(concreteRefEquals);
            try
            {
                // https://stackoverflow.com/questions/5449863/how-to-get-the-count-from-iqueryable
                var query = _ciSet.AsNoTracking().Where(a => ((classEntityId == null ? true : a.ClassEntityId == classEntityId)
                                                              && (ownerId == null ? true : a.OwnerId == ownerId)
                                                              && (skipNameLike || a.Name.ToLower().Contains(nameLike.ToLower()))
                                                              && (skipNameEquals || a.Name.ToLower().Equals(nameEquals.ToLower()))
                                                              && (skipConcreteRef || a.ConcreteReference.Equals(concreteRefEquals))
                                                ))
                            .Include(i => i.SourceRelationships)
                            .Include(i => i.TargetRelationships)
                            .OrderBy(o => o.Name)
                            .AsQueryable();

                pager.TotalRecordCount = query.Count();
                //return query.Skip(startRowIndex).Take(maximumRows);
                var reply = query.Skip(pager.StartRowIndex).Take(pager.MaxPageSize).ToList<ConfigItemEntity>();
                pager.CurrentResultCount = reply.Count();
                return reply;
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

                var cir = _cirSet.AsNoTracking().Where(p => p.SourceConfigItemEntityId == sourceConfigItemId)
                            .ToList();
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

        public bool DeleteConfigItemRelationships(IEnumerable<ConfigItemRelationshipEntity> configItemRelationships, string userName)
        {
            
            foreach(var configItemRelationship in configItemRelationships)
            {
                DeleteConfigItemRelationship(configItemRelationship.Id, userName);
            }
            return true;
        }

        public bool DeleteConfigItemRelationship(Guid configItemRelationshipId, string userName)
        {
            try
            {              
                ConfigItemRelationshipEntity cir = _cirSet.Where(p => p.Id == configItemRelationshipId).FirstOrDefault();
                _dbContext.Entry(cir).State = EntityState.Modified;
                cir.DeletedOn = DateTime.Now;
                cir.DeletedBy = userName;
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new DataWriteException(e.Message, e);
            }
        }

        public void DeleteConfigItemRelationshipsToClass(Guid sourceConfigItemEntityId, Guid targetClassEntityId, string userName)
        {

            try
            {
                List <ConfigItemRelationshipEntity> relList = _cirSet.AsNoTracking().Where(p => p.SourceConfigItemEntityId == sourceConfigItemEntityId
                && p.TargetConfigItem.ClassEntityId==targetClassEntityId).ToList();

                foreach(var rel in relList)
                {
                    this.DeleteConfigItemRelationship(rel.Id, userName);
                }

            }
            catch (Exception e)
            {
                throw new DataReadException(e.Message, e);
            }
        }


    }
}
