using System.Text;
using System.Security.Cryptography;

namespace COMMSFinalProject.Models
{
    public class User
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int Age { get; set; }
        public int Salary { get; set; }
        public string Email { get; set; }
        public bool isBlocked { get; set; }
        public string Password { get; set; }
        public List<Loan> Loans { get; set; } = new List<Loan>();
        public User()
        {
            isBlocked = false;
        }

    }

    public class Role
    {
        public const string User = "User";
        public const string Accountant = "Accountant";
    }
}
