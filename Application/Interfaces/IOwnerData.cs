using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSCiR.Model;

namespace Application.Interfaces
{
    public interface IOwnerData
    {
        OwnerEntity Create(OwnerEntity ownerEntity);
        OwnerEntity Read(Guid ownerGuid);
        OwnerEntity Update(OwnerEntity ownerEntity);
        bool Delete(Guid ownerGuid);

        IEnumerable<OwnerEntity> GetOwners(string ownerCodeEquals, string ownerNameContains);

    }
}
