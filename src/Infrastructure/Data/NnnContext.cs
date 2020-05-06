using Microsoft.EntityFrameworkCore;
using Microsoft.Nnn.ApplicationCore.Entities.Categories;
using Microsoft.Nnn.ApplicationCore.Entities.CommentLikes;
using Microsoft.Nnn.ApplicationCore.Entities.Comments;
using Microsoft.Nnn.ApplicationCore.Entities.Communities;
using Microsoft.Nnn.ApplicationCore.Entities.CommunityUsers;
using Microsoft.Nnn.ApplicationCore.Entities.Likes;
using Microsoft.Nnn.ApplicationCore.Entities.Notifications;
using Microsoft.Nnn.ApplicationCore.Entities.PostCategories;
using Microsoft.Nnn.ApplicationCore.Entities.Posts;
using Microsoft.Nnn.ApplicationCore.Entities.PostTags;
using Microsoft.Nnn.ApplicationCore.Entities.ReplyLikes;
using Microsoft.Nnn.ApplicationCore.Entities.Unlikes;
using Microsoft.Nnn.ApplicationCore.Entities.Users;

namespace Microsoft.Nnn.Infrastructure.Data
{
    //dotnet ef migrations add linkUrl --context NnnContext -p ../Infrastructure/Infrastructure.csproj -s Web.csproj -o Data/Migrations
    public class NnnContext : DbContext
    {
        public NnnContext(DbContextOptions<NnnContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<ReplyLike> ReplyLikes { get; set; }
        public DbSet<Unlike> Unlikes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> PostTags { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }
        public DbSet<Community> Communities { get; set; }
        public DbSet<CommunityUser> CommunityUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PostTag>()
                .HasOne<Post>(sc => sc.Post)
                .WithMany(s => s.Tags)
                .HasForeignKey(sc => sc.PostId);
            
            builder.Entity<CommunityUser>()
                .HasOne<Community>(sc => sc.Community)
                .WithMany(s => s.Users)
                .HasForeignKey(sc => sc.CommunityId);
        }
        
    }
}
