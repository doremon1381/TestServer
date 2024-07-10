using DTBOAuthLoginService.Database.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DTBOAuthLoginService.Database
{
    internal class CustomClientDbServices : DbContextBase<CustomClient>, ICustomClientDbServices
    {
        private DbSet<CustomClient> _CustomClients { get; set; }
        public CustomClientDbServices(ILAuthenticationDbContext dbContext) : base(dbContext)
        {
            _CustomClients = this._DbModels;
        }
    }

    internal interface ICustomClientDbServices
    {
    }
}
