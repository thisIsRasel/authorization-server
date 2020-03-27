using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }
    }
}
