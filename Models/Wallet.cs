public class Wallet
{
    public int WalletId { get; set; }
    public decimal Balance { get; set; }
    
    // Foreign Key
    public int UserId { get; set; }

    // Navigation Property
    public User User { get; set; } = null!;
}
