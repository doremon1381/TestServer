using Duende.IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTBOAuthLoginService.Database.Model
{
    [PrimaryKey(nameof(ID))]
    public class ClientClaim : Duende.IdentityServer.Models.ClientClaim, IDbTable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
    }
}
