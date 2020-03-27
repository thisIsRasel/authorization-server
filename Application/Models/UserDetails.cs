
namespace Application.Models
{
    public class UserDetails
    {
        public int Id { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }

        public string[] Roles { get; set; }
    }
}
