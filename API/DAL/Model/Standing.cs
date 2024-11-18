namespace API.DAL.Model;

public partial class Standing {
    public int? Points { get; set; }

    public int ContestId { get; set; }

    public int UserId { get; set; }

    public virtual Contest Contest { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
