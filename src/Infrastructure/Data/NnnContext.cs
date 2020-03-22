using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Categories;
using Microsoft.Nnn.ApplicationCore.Entities.Comments;
using Microsoft.Nnn.ApplicationCore.Entities.Likes;
using Microsoft.Nnn.ApplicationCore.Entities.Notifications;
using Microsoft.Nnn.ApplicationCore.Entities.PostCategories;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.PostTags;
using Microsoft.Nnn.ApplicationCore.Entities.Unlikes;
using Microsoft.Nnn.ApplicationCore.Entities.Users;

namespace Microsoft.Nnn.Infrastructure.Data
{
    //dotnet ef migrations add post_categories --context NnnContext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -o Data/Migrations
    public class NnnContext : DbContext
    {
        public NnnContext(DbContextOptions<NnnContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Unlike> Unlikes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PostTag>()
                .HasOne<Post>(sc => sc.Post)
                .WithMany(s => s.Tags)
                .HasForeignKey(sc => sc.PostId);
            
            builder.Entity<PostCategory>()
                .HasOne<Post>(sc => sc.Post)
                .WithMany(s => s.Categories)
                .HasForeignKey(sc => sc.PostId);
        }
        
    }
}
