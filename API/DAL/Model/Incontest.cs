namespace API.DAL.Model;

public partial class Incontest {
    public int ContestId { get; set; }

    public int MatchId { get; set; }

    public virtual Contest Contest { get; set; } = null!;

    public virtual Match Match { get; set; } = null!;
}
