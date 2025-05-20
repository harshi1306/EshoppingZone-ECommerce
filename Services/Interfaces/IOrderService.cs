using EshoppingZoneAPI.DTOs;
using EshoppingZoneAPI.Models;

namespace EshoppingZoneAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Order> PlaceOrderAsync(int userId, CheckoutDTO dto);
    }
}
