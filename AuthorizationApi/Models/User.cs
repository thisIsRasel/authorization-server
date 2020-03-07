using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizationApi.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }

        [NotMapped]
        public string[] Roles { get; set; }
    }
}
