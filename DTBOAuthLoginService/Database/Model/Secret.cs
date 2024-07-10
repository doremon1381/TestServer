using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTBOAuthLoginService.Database.Model
{
    [PrimaryKey(nameof(ID))]
    public class Secret: Duende.IdentityServer.Models.Secret, IDbTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }
}
