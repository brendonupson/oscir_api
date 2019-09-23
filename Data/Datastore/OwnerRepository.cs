using System;
using System.Collections.Generic;
using System.Linq;
using OSCiR.Model;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using DomainLayer.Exceptions;

namespace OSCiR.Datastore
{
    public class OwnerRepository : IOwnerData
    {
        private DbContext _dbContext;
        private DbSet<OwnerEntity> _ownerSet { get; set; }

        private DbSet<ConfigItemEntity> _configItemSet { get; set; }


        public OwnerRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _ownerSet = _dbContext.Set<OwnerEntity>();

            _configItemSet = _dbContext.Set<ConfigItemEntity>();
        }

        public OwnerEntity Read(Guid ownerGuid)
        {

            if (ownerGuid==Guid.Empty) throw new DataReadException("Invalid Guid passed to Read()");

            try
            {
                OwnerEntity owner = _ownerSet.AsNoTracking().Where(p => p.Id == ownerGuid).FirstOrDefault();
                return owner;
            }
            catch (Exception e)
            {
                throw new DataReadException(e.Message, e);
            }
        }

        public bool Delete(Guid ownerGuid)
        {
            try
            {
                OwnerEntity ownerToBeDeleted = new OwnerEntity { Id = ownerGuid };
                _dbContext.Entry(ownerToBeDeleted).State = EntityState.Deleted;
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DataWriteException("Delete(): " + e.Message, e);
            }

            return true;
        }

        public IEnumerable<OwnerEntity> GetOwners(string ownerCodeEquals, string ownerNameContains, bool getUsedOwnersOnly)
        {

            try
            {
                var ownerResults = _ownerSet.Where(a => (String.IsNullOrEmpty(ownerCodeEquals) || a.OwnerCode.ToLower().Equals(ownerCodeEquals.ToLower())) &&
                                                        (String.IsNullOrEmpty(ownerNameContains) || a.OwnerName.ToLower().Contains(ownerNameContains.ToLower())) &&
                                                        (!getUsedOwnersOnly || _configItemSet.Where(ci => ci.OwnerId == a.Id).Count() > 0))
                            .OrderBy(o => o.OwnerCode)
                            .ThenBy(x => x.OwnerName)
                            .AsNoTracking().ToList<OwnerEntity>();
                return ownerResults;

            }
            catch (Exception e)
            {
                throw new DataReadException("GetOwners(): " + e.Message, e);
            }
        }

        public IEnumerable<OwnerEntity> GetOwners(Guid? ownerEntityId)
        {
            try
            {
                var ownerResults = _ownerSet.Where(a => (ownerEntityId==null || ownerEntityId==a.Id))
                            .OrderBy(o => o.OwnerCode)
                            .ThenBy(x => x.OwnerName)
                            .AsNoTracking().ToList<OwnerEntity>();
                return ownerResults;

            }
            catch (Exception e)
            {
                throw new DataReadException("GetOwners(): " + e.Message, e);
            }
        }

        public OwnerEntity Update(OwnerEntity ownerEntity)
        {

            try
            {
                ownerEntity.ModifiedOn = DateTime.Now;

                _dbContext.Entry(ownerEntity).State = EntityState.Modified;
                _dbContext.Entry(ownerEntity).Property(x => x.CreatedBy).IsModified = false;
                _dbContext.Entry(ownerEntity).Property(x => x.CreatedOn).IsModified = false;
                _dbContext.Entry(ownerEntity).Property(x => x.Id).IsModified = false;

                _dbContext.SaveChanges();
                _dbContext.Entry(ownerEntity).Reload();

                return ownerEntity;
            }
            catch (Exception e)
            {
                throw new DataWriteException("Update(): " + e.Message, e);
            }
        }

        public OwnerEntity Create(OwnerEntity ownerEntity)
        {

            try
            {
                _ownerSet.Add(ownerEntity);
                _dbContext.SaveChanges();
                _dbContext.Entry(ownerEntity).Reload();
                return ownerEntity;
            }
            catch (Exception e)
            {
                throw new DataWriteException("Create(): " + e.Message, e);
            }
        }
    }
}