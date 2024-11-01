namespace FrontEnd.ViewModel {
    public class ContestVM {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public sbyte? IsOpened { get; set; }
    }
}
