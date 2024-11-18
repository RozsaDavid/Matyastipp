namespace API.DAL.Model;

public partial class Bet {
    public int Id { get; set; }

    public int ContestId { get; set; }

    public int UserId { get; set; }

    public int MatchId { get; set; }

    public int HomeGoals { get; set; }

    public int GuestGoals { get; set; }

    public sbyte? IsWon { get; set; }

    public virtual Contest Contest { get; set; } = null!;

    public virtual Match Match { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
