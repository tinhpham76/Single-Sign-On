﻿using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SSO.Backend.Data.Entities;

namespace SSO.Backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //Db set table client
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientClaim> ClientClaims { get; set; }
        public DbSet<ClientRedirectUri> ClientRedirectUris { get; set; }
        public DbSet<ClientPostLogoutRedirectUri> ClientPostLogoutRedirectUris { get; set; }
        public DbSet<ClientCorsOrigin> ClientCorsOrigins { get; set; }
        public DbSet<ClientGrantType> ClientGrantTypes { get; set; }
        public DbSet<ClientScope> ClientScopes { get; set; }
        public DbSet<ClientSecret> ClientSecrets { get; set; }
        public DbSet<ClientIdPRestriction> ClientIdPRestrictions { get; set; }
        public DbSet<ClientProperty> ClientProperties { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
            builder.Entity<User>().Property(x => x.Id).HasMaxLength(50).IsUnicode(false);
        }
    }
}
