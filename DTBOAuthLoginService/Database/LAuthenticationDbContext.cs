using DTBOAuthLoginService.Database.Model;
using Duende.IdentityServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace DTBOAuthLoginService.Database
{
    public class LAuthenticationDbContext : DbContext, ILAuthenticationDbContext
    {
        private ILogger<LAuthenticationDbContext> _logger;
        private DbContextOptions _options;
        //private IConfiguration _configuration;

        #region DbSet needs to be add in this DbContext to prevent an error of DbSet is not existed in this context (I think it means in this DbContext class) when using later
        private DbSet<CustomClient> CustomClients { get; set; }
        private DbSet<Model.ClientClaim> ClientClaims { get; set; }
        private DbSet<Model.Secret> Secret { get; set; }
        #endregion

        public LAuthenticationDbContext(DbContextOptions<LAuthenticationDbContext> options, ILogger<LAuthenticationDbContext> logger)
            : base(options)
        {
            _options = options;
            _logger = logger;

            //_configuration = configuration;

        }

        public void DbSaveChanges()
        {
            this.SaveChanges();
        }

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            if (typeof(IDbTable).IsAssignableFrom(typeof(TEntity)))
            {
                return this.Set<TEntity>();
            }
            
            // TODO: for learning
            _logger.LogInformation($"GetDbSet is called!");
            // TODO: will change 
            return null;
        }

#if (DEBUG || RELEASE)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CustomClient>()
            //    .Property(x => x.Properties)
            //    .HasConversion(
            //        x => JsonConvert.SerializeObject(x),
            //        x => JsonConvert.DeserializeObject<IDictionary<string, string>>(x),
            //        // TODO: need to learn about compare
            //        new ValueComparer<IDictionary<string, string>>(
            //            (c1, c2) => c1.SequenceEqual(c2),
            //            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            //            c => c.ToDictionary<string,string>()));

            base.OnModelCreating(modelBuilder);
        }
#else 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-2TRDKFE\\;Database=LAuthentication;trusted_connection=true;TrustServerCertificate=True");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<CustomClient>()
            //    .Property(x => x.Properties)
            //    .HasConversion(
            //        x => JsonConvert.SerializeObject(x),
            //        x => JsonConvert.DeserializeObject<IDictionary<string, string>>(x));
            //        // TODO: need to learn about compare
            //        new ValueComparer<IDictionary<string, string>>(
            //            (c1, c2) => c1.SequenceEqual(c2),
            //            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            //            c => c.ToDictionary<string,string>()));
            base.OnModelCreating(modelBuilder);
        }
#endif
    }

    public interface ILAuthenticationDbContext
    {
        void DbSaveChanges();
        DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class;
    }

    public class YourDbContextFactory : IDesignTimeDbContextFactory<LAuthenticationDbContext>
    {
        public LAuthenticationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LAuthenticationDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-2TRDKFE\\;Database=LAuthentication;trusted_connection=true;TrustServerCertificate=True");

            // TODO: logger as parameter is null for now
            return new LAuthenticationDbContext(optionsBuilder.Options, null);
        }
    }
}
