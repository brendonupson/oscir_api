using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSCiR.Model;

namespace Application.Interfaces
{
    public interface IUserData
    {
        UserEntity Create(UserEntity userEntity);
        UserEntity Read(Guid userGuid);
        UserEntity Update(UserEntity userEntity, bool setPasswordHash);
        bool Delete(Guid userGuid);

        UserEntity GetByUserName(string username);
        Task<int> GetUserCountAsync();
        IEnumerable<UserEntity> GetAllUsers();
    }
}
