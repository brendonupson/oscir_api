using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using OSCiR.Model;
using Microsoft.EntityFrameworkCore;
using DomainLayer.Exceptions;

namespace OSCiR.Datastore
{
    public class UserRepository : IUserData
    {
        private DbContext _dbContext;
        private DbSet<UserEntity> _userSet { get; set; }


        public UserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _userSet = _dbContext.Set<UserEntity>();
        }

        public async Task<int> GetUserCountAsync()
        {
            int count = await _userSet.CountAsync();
            return count;
        }



        public IEnumerable<UserEntity> GetAllUsers()
        {
            var users = _userSet.OrderBy(o => o.Username);

            return users;
        }


        public UserEntity Read(Guid userGuid)
        {
            if (userGuid == null || userGuid == Guid.Empty) throw new DataReadException("Invalid Guid passed to Read()");

            try
            {
                UserEntity user = _userSet.Where(p => p.Id == userGuid).First();

                return user;
            }
            catch (Exception e)
            {
                throw new DataReadException(e.Message, e);
            }
        }


        public UserEntity Create(UserEntity userEntity)
        {
           
            try
            {
                _userSet.Add(userEntity);
                _dbContext.SaveChanges();
                _dbContext.Entry(userEntity).Reload();

                return userEntity;
            }
            catch (Exception e)
            {
                throw new DataWriteException("Create(): " + e.Message, e);
            }
        }

        public UserEntity Update(UserEntity userEntity, bool updatePasswordHash)
        {
            try
            {
                userEntity.ModifiedOn = DateTime.Now;

                _dbContext.Entry(userEntity).State = EntityState.Modified;
                if(!updatePasswordHash)
                {
                    _dbContext.Entry(userEntity).Property(x => x.PasswordHash).IsModified = false;
                }
                userEntity.Token = null; //NotMapped

                _dbContext.Entry(userEntity).Property(x => x.CreatedBy).IsModified = false;
                _dbContext.Entry(userEntity).Property(x => x.CreatedOn).IsModified = false;
                _dbContext.Entry(userEntity).Property(x => x.Id).IsModified = false;

                _dbContext.SaveChanges();
                _dbContext.Entry(userEntity).Reload();
                userEntity.PasswordHash = null;
                return userEntity;
            }
            catch (Exception e)
            {
                throw new DataWriteException("Update(): " + e.Message, e);
            }
        }

        public bool Delete(Guid userGuid)
        {
            if (userGuid == null || userGuid == Guid.Empty) throw new DataWriteException("Invalid Guid passed to Delete()");

            try
            {
                UserEntity userToBeDeleted = new UserEntity { Id = userGuid };
                _dbContext.Entry(userToBeDeleted).State = EntityState.Deleted;
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                throw new DataWriteException("Delete(): " + e.Message, e);
            }

            return true;
        }

        public UserEntity GetByUserName(string username)
        {
            return _userSet.AsNoTracking().SingleOrDefault(x => x.Username.ToLower() == username.ToLower());
        }
    }
}
