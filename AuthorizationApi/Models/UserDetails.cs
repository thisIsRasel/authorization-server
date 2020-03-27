using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizationApi.Models
{
    public class UserDetails
    {
        public int Id { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }

        public string[] Roles { get; set; }
    }
}
