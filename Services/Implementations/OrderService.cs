// OrderService.cs
using EshoppingZoneAPI.DTOs;
using EshoppingZoneAPI.Models;
using EshoppingZoneAPI.Repositories.Interfaces;
using EshoppingZoneAPI.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EshoppingZoneAPI.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepo;
        private readonly IRepository<CartItem> _cartRepo;
        private readonly IRepository<Product> _productRepo;
        private readonly IRepository<OrderItem> _orderItemRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Wallet> _walletRepo;

        public OrderService(
            IRepository<Order> orderRepo,
            IRepository<CartItem> cartRepo,
            IRepository<Product> productRepo,
            IRepository<OrderItem> orderItemRepo,
            IRepository<User> userRepo,
            IRepository<Wallet> walletRepo
        )
        {
            _orderRepo = orderRepo;
            _cartRepo = cartRepo;
            _productRepo = productRepo;
            _orderItemRepo = orderItemRepo;
            _userRepo = userRepo;
            _walletRepo = walletRepo;
        }

        public async Task<Order> PlaceOrderAsync(int userId, CheckoutDTO dto)
        {
            var cartItems = await _cartRepo.FindAsync(c => c.UserId == userId);
            if (cartItems.Count == 0)
                throw new Exception("Cart is empty.");

            decimal total = 0;

            foreach (var item in cartItems)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                if (product == null || product.Quantity < item.Quantity)
                    throw new Exception("Invalid cart item or insufficient stock.");

                total += product.Price * item.Quantity;
            }

            //  Wallet Payment Deduction
            if (dto.PaymentMethod == "Wallet")
            {
                var wallet = (await _walletRepo.FindAsync(w => w.UserId == userId)).FirstOrDefault();
                if (wallet == null || wallet.Balance < total)
                    throw new Exception("Insufficient wallet balance.");

                wallet.Balance -= total;
                await _walletRepo.SaveChangesAsync();
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = total,
                PaymentMethod = dto.PaymentMethod
            };

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveChangesAsync();

            foreach (var item in cartItems)
            {
                var product = await _productRepo.GetByIdAsync(item.ProductId);
                product.Quantity -= item.Quantity;
                await _productRepo.SaveChangesAsync();

                var orderItem = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                };

                await _orderItemRepo.AddAsync(orderItem);
            }

            await _orderItemRepo.SaveChangesAsync();
            await _cartRepo.DeleteRangeAsync(cartItems); //  Fixed error
            await _cartRepo.SaveChangesAsync();

            var user = await _userRepo.GetByIdAsync(userId);
            await SendEmailAsync(user.Email, order.OrderId, total);

            return order;
        }

        public async Task SendEmailAsync(string toEmail, int orderId, decimal total)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("eshoppingzonenotification@gmail.com"));
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = "Order Confirmation";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
                {
                    Text = $"Your order #{orderId} of ₹{total} has been placed successfully!"
                };

                using var smtp = new SmtpClient();

                // Connect to the Gmail SMTP server
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

                // Authenticate with the Gmail account
                await smtp.AuthenticateAsync("eshoppingzonenotification@gmail.com", "dzjb fvzo mprt kxzg"); 

                // Send the email
                await smtp.SendAsync(email);

                // Disconnect after sending the email
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw new Exception("There was an issue sending the email.");
            }
        }

    }
}
