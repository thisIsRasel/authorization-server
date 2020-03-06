using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizationApi.Models
{
    [Table("user_role_maps")]
    public class UserRoleMap
    {
        [Key]
        public long Id { get; set; }
        
        [Column("user_id")]
        public long UserId { get; set; }
        
        public string Role { get; set; }
    }
}
