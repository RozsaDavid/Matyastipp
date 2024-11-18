namespace API.DAL.Model;

public partial class User {
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string RealName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public sbyte? IsActive { get; set; }

    public sbyte? IsAdmin { get; set; }

    public virtual ICollection<Bet> Bets { get; set; } = new List<Bet>();
}
