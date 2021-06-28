using BackEndAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackEndAPI.DBContext
{
    public class AssetsManagementDBContext : IdentityDbContext<User, Role, int>
    {
        public AssetsManagementDBContext(DbContextOptions<AssetsManagementDBContext> options)
             : base(options)
        {

        }

        public DbSet<AssetCategory> AssetCategories { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<ReturnRequest> ReturnRequest { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AssetConfiguration).Assembly);

            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaim");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRole").HasKey(x => new { x.UserId, x.RoleId });
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins").HasKey(x => new { x.UserId });
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens").HasKey(x => new { x.UserId });
        }
    }
}