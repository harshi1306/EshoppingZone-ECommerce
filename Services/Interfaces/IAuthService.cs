using EshoppingZoneAPI.DTOs;
using EshoppingZoneAPI.Models;

namespace EshoppingZoneAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string?> Register(UserRegisterDTO dto);
        Task<string?> Login(UserLoginDTO dto);
    }
}
