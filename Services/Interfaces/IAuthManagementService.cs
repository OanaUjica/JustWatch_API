using Lab1_.NET.ViewModels.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab1_.NET.Services
{
    public interface IAuthManagementService
    {
        Task<ServiceResponse<RegisterResponse, IEnumerable<IdentityError>>> RegisterUser(RegisterRequest registerRequest);

        Task<bool> ConfirmUserRequest(ConfirmUserRequest confirmUserRequest);

        Task<ServiceResponse<LoginResponse, string>> LoginUser(LoginRequest loginRequest);
    }
}
