namespace FrontEnd.ViewModel {
    public class MatchVM {
        public int Id { get; set; }

        public string HomeName { get; set; } = null!;

        public string GuestName { get; set; } = null!;

        public DateTime Date { get; set; }

        public int? HomeGoals { get; set; }

        public int? GuestGoals { get; set; }

        public sbyte? IsAvailable { get; set; }
    }
}
