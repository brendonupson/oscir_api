using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using OSCiR.Model;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OSCiR.Shared;

namespace App
{
    public class OwnerManager
    {
        IOwnerData _ownerRepo;
       

        public OwnerManager(IOwnerData ownerRepo)
        {
            _ownerRepo = ownerRepo;
        }

       


        public OwnerEntity Read(Guid id)
        {
            return _ownerRepo.Read(id);
        }


        public OwnerEntity Update(OwnerEntity ownerEntity)
        {
            return _ownerRepo.Update(ownerEntity);
        }

        public IEnumerable<OwnerEntity> GetOwners(string ownerCodeEquals, string ownerNameContains)
        {
            var owners = _ownerRepo.GetOwners(ownerCodeEquals, ownerNameContains);
            return owners;
        }

        public OwnerEntity Create(OwnerEntity ownerEntity)
        {
            return _ownerRepo.Create(ownerEntity);
        }

        public bool Delete(Guid ownerGuid)
        {
            return _ownerRepo.Delete(ownerGuid);
        }
    }
}
