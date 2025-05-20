using EshoppingZoneAPI.Models;
using EshoppingZoneAPI.Repositories.Interfaces;
using EshoppingZoneAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EshoppingZoneAPI.Services.Implementations
{
    public class WalletService : IWalletService
    {
        private readonly IRepository<Wallet> _walletRepo;
        private readonly IRepository<User> _userRepo;

        public WalletService(IRepository<Wallet> walletRepo, IRepository<User> userRepo)
        {
            _walletRepo = walletRepo;
            _userRepo = userRepo;
        }

        public async Task<Wallet> GetWalletByUserIdAsync(int userId)
        {
            var wallet = await _walletRepo.FindAsync(w => w.UserId == userId);
            return wallet.FirstOrDefault();
        }

        public async Task<Wallet> AddFundsAsync(int userId, decimal amount)
        {
            var wallet = await GetWalletByUserIdAsync(userId);
            if (wallet == null)
            {
                wallet = new Wallet
                {
                    UserId = userId,
                    Balance = amount
                };
                await _walletRepo.AddAsync(wallet);
            }
            else
            {
                wallet.Balance += amount;
            }
            
            await _walletRepo.SaveChangesAsync();
            return wallet;
        }
    }
}
