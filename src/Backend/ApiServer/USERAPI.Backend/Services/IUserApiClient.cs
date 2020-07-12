using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USERAPI.Backend.Models;

namespace USERAPI.Backend.Services
{
    public interface IUserApiClient
    {
        Task<UserViewModel> GetById(string id);
        Task<bool> PutUser(string id, UserUpdateRequest request);
        Task<bool> ChangePassword(string id, UserPasswordChangeRequest request);
        Task<bool> DeleteUser(string id);

    }
}
