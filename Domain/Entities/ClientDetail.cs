using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("client_details")]
    public class ClientDetail
    {
        [Key]
        public long Id { get; set; }

        [Column("client_id")]
        public string ClientId { get; set; }

        [Column("client_secret")]
        public string ClientSecret { get; set; }

        [Column("redirect_url")]
        public string RedirectUrl { get; set; }
    }
}
