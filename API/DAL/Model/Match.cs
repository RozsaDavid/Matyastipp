using System;
using System.Collections.Generic;

namespace API.DAL.Model;

public partial class Match
{
    public int Id { get; set; }

    public string HomeName { get; set; } = null!;

    public string GuestName { get; set; } = null!;

    public DateTime Date { get; set; }

    public int? HomeGoals { get; set; }

    public int? GuestGoals { get; set; }

    public sbyte? IsAvailable { get; set; }

    public virtual ICollection<Bet> Bets { get; set; } = new List<Bet>();
}
