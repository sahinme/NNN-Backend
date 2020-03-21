using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Categories;
using Microsoft.Nnn.ApplicationCore.Entities.Notifications;
using Microsoft.Nnn.ApplicationCore.Entities.Users;

namespace Microsoft.Nnn.Infrastructure.Data
{
    //dotnet ef migrations add initial --context NnnContext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -o Data/Migrations
    public class NnnContext : DbContext
    {
        public NnnContext(DbContextOptions<NnnContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
        }
        
    }
}
