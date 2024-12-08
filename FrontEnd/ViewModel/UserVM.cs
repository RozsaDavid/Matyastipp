using System.ComponentModel.DataAnnotations;

namespace FrontEnd.ViewModel {
    public class UserVM {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        /*[RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
            ErrorMessage = "Adj meg érvényes jelszót! (Minimum 8 karakter, kisbetű, nagybetű, szám és speciális karaktert tartalmaznia kell.)")]*/
        public string Password { get; set; } = null!;

        public string RealName { get; set; } = null!;

        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?",
            ErrorMessage = "Adj meg érvényes e-mail címet, pl.: \"pelda@domain.com\" .")]
        public string Email { get; set; } = null!;

        public sbyte? IsActive { get; set; }

        public sbyte? IsAdmin { get; set; }
    }
}
