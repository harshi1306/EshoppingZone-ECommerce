using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EshoppingZoneAPI.Services.Interfaces;
using EshoppingZoneAPI.DTOs;
using System.Security.Claims;





namespace EshoppingZoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("balance")]
        public async Task<IActionResult> GetBalance()
        {
            var wallet = await _walletService.GetWalletByUserIdAsync(GetUserId());
            if (wallet == null)
                return NotFound("Wallet not found.");

            return Ok(new { Balance = wallet.Balance });
        }

        [HttpPost("add-funds")]
        public async Task<IActionResult> AddFunds([FromBody] AddFundsDTO dto)
        {
            if (dto.Amount <= 0) return BadRequest("Amount must be greater than zero.");

            var wallet = await _walletService.AddFundsAsync(GetUserId(), dto.Amount);
            return Ok(new { Balance = wallet.Balance });
        }
    }
}
