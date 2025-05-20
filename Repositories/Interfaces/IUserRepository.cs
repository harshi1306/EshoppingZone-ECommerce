using EshoppingZoneAPI.Models;

namespace EshoppingZoneAPI.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
