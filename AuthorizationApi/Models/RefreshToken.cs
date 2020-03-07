using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthorizationApi.Models
{
    [Table("refresh_tokens")]
    public class RefreshToken
    {
        [Key]
        public long Id { get; set; }

        public string Token { get; set; }
    }
}
