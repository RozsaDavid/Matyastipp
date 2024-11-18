namespace API.DAL.Model;

public partial class Scoringrule {
    public string Description { get; set; } = null!;

    public int Points { get; set; }

    public int ContestId { get; set; }

    public virtual Contest Contest { get; set; } = null!;
}
