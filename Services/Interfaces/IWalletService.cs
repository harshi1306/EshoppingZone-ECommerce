using EshoppingZoneAPI.Models;

namespace EshoppingZoneAPI.Services.Interfaces
{
    public interface IWalletService
    {
        Task<Wallet> GetWalletByUserIdAsync(int userId);
        Task<Wallet> AddFundsAsync(int userId, decimal amount);
    }
}
