namespace FrontEnd.ViewModel {
    public class BetVM {
        public int Id { get; set; }

        public int ContestId { get; set; }

        public int UserId { get; set; }

        public int MatchId { get; set; }

        public int HomeGoals { get; set; }

        public int GuestGoals { get; set; }

        public sbyte? IsWon { get; set; }
    }
}
