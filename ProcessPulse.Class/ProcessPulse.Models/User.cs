using ProcessPulse.Class.ProcessPulse.Models;

namespace ProcessPulse.Class.ProcessPulse.Models
{
    public class User
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firma { get; set; }
        public string IpKomputera { get; set; }
    }
}
public class UserManager
{
    public User GetCurrentUser()
    {
        // Tu zaimplementuj logikę pozyskiwania danych zalogowanego użytkownika
        return new User
        {
            Email = "example@example.com",
            Imie = "Jan",
            Nazwisko = "Kowalski",
            Password = "password123",
            Firma = "ABC Corp",
            IpKomputera = "192.168.1.1"
        };
    }
}
