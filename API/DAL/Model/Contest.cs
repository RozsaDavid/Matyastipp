using System;
using System.Collections.Generic;

namespace API.DAL.Model;

public partial class Contest
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public sbyte? IsOpened { get; set; }

    public virtual ICollection<Bet> Bets { get; set; } = new List<Bet>();
}
