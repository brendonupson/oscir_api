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


        public OwnerRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _ownerSet = _dbContext.Set<OwnerEntity>();
        }

        public OwnerEntity Read(Guid ownerGuid)
        {

            if (ownerGuid == null || ownerGuid==Guid.Empty) throw new DataReadException("Invalid Guid passed to Read()");

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

        public IEnumerable<OwnerEntity> GetOwners(string ownerCodeContains, string ownerNameContains)
        {

            try
            {
                //this seems a bit rubbish. Need to see if Linq can do dynamic queries based on input parameters
                if (ownerCodeContains != null && ownerCodeContains.Length > 0 &&
                ownerNameContains != null && ownerNameContains.Length > 0)
                {
                    var ownerResults = _ownerSet.Where(a => (a.OwnerCode.ToLower().Contains(ownerCodeContains.ToLower()) &&
                            a.OwnerName.ToLower().Contains(ownerNameContains.ToLower())))
                            .OrderBy(o => o.OwnerCode)
                            .ThenBy(x => x.OwnerName)
                            .AsNoTracking().ToList<OwnerEntity>();
                    return ownerResults;
                }


                if (ownerCodeContains != null && ownerCodeContains.Length > 0)
                {
                    var ownerResults = _ownerSet.Where(a => a.OwnerCode.ToLower().Contains(ownerCodeContains.ToLower()))
                            .OrderBy(o => o.OwnerCode)
                            .ThenBy(x => x.OwnerName)
                            .AsNoTracking().ToList<OwnerEntity>();
                    return ownerResults;
                }

                if (ownerNameContains != null && ownerNameContains.Length > 0)
                {
                    var ownerResults = _ownerSet.Where(a => a.OwnerName.ToLower().Contains(ownerNameContains.ToLower()))
                            .OrderBy(o => o.OwnerCode)
                            .ThenBy(x => x.OwnerName)
                            .AsNoTracking().ToList<OwnerEntity>();
                    return ownerResults;
                }


                var result = _ownerSet
                            .OrderBy(o => o.OwnerCode)
                            .ThenBy(x => x.OwnerName)
                            .AsNoTracking().ToList<OwnerEntity>();
                return result;
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