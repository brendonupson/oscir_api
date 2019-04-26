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
    public class UserManager
    {
        IUserData _userRepo;
        string _secret;

        public UserManager(IUserData userRepo, string secret)
        {
            _userRepo = userRepo;
            _secret = secret;
        }

        public UserEntity GenerateDefaultUser(string password)
        {
            UserEntity user = new UserEntity()
            {
                FirstName = "",
                LastName = "Admin",
                Username = "admin",
                ModifiedBy = "SETUP",
                CreatedBy = "SETUP",
                IsAdmin = true
            };
            user.SetPassword(password);

            user = _userRepo.Create(user);

            return user;
        }

        public async Task<int> GetUserCountAsync()
        {
            return await _userRepo.GetUserCountAsync();
        }

        public string GenerateToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            if (user.IsAdmin) tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, UserRoles.Admin));

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public UserEntity GetUser(Guid id)
        {
            return _userRepo.Read(id);
        }

        public UserEntity Authenticate(string username, string password)
        {
            if (username == null || password == null) return null;

            var user = _userRepo.GetByUserName(username.ToLower());

            // return null if user not found
            if (user == null) return null;

            if (!user.IsPasswordMatch(password)) return null;

            var previousLogin = user.LastLogin;

            try
            {
                var updateUser = _userRepo.Read(user.Id);
                updateUser.LastLogin = DateTime.Now;

                _userRepo.Update(updateUser, false);               
            }
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch (Exception) { } //ignore any errors
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body

            user.LastLogin = previousLogin;

            user.Token = GenerateToken(user);
            // remove password before returning
            user.PasswordHash = null;

            return user;
        }

        public UserEntity UpdateUser(UserEntity userEntity, bool updatePassword)
        {
            return _userRepo.Update(userEntity, updatePassword);
        }

        public IEnumerable<UserEntity> GetAllUsers()
        {
            List<UserEntity> returnUsers = new List<UserEntity>();
            var users = _userRepo.GetAllUsers();
            foreach (UserEntity user in users)
            {
                //probably not the most efficient.... fine for a small number of user accounts
                var serialized = JsonConvert.SerializeObject(user);
                var cloneUser = JsonConvert.DeserializeObject<UserEntity>(serialized);
                cloneUser.PasswordHash = null;
                returnUsers.Add(cloneUser);
            }
            return returnUsers;
        }

        public UserEntity Create(UserEntity userEntity)
        {
            return _userRepo.Create(userEntity);
        }

        public bool Delete(Guid id)
        {
            return _userRepo.Delete(id);
        }
    }
}
