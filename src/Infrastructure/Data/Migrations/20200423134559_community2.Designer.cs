﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Nnn.Infrastructure.Data;

namespace Microsoft.Nnn.Infrastructure.Data.Migrations
{
    [DbContext(typeof(NnnContext))]
    [Migration("20200423134559_community2")]
    partial class community2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Categories.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<string>("DisplayName");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Comments.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<long>("PostId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Communities.Community", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Communities");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.CommunityUsers.CommunityUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CommunityId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<bool>("Suspended");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CommunityId");

                    b.HasIndex("UserId");

                    b.ToTable("CommunityUsers");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Likes.Like", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<long>("PostId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Notifications.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<long>("ContentId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsRead");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<int>("NotifyContentType");

                    b.Property<long>("OwnerId");

                    b.Property<int>("OwnerType");

                    b.Property<long>("SenderId");

                    b.Property<int>("SenderType");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.PostCategories.PostCategory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CategoryId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<long>("PostId");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("PostId");

                    b.ToTable("PostCategories");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.PostTags.PostTag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<long>("PostId");

                    b.Property<long>("TagId");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTags");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.PostTags.Tag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Text")
                        .HasMaxLength(32);

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Posts.Post", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CommunityId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<string>("How");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Title");

                    b.Property<long>("UserId");

                    b.Property<string>("Where");

                    b.Property<string>("Why");

                    b.HasKey("Id");

                    b.HasIndex("CommunityId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Replies.Reply", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("CommentId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.HasIndex("UserId");

                    b.ToTable("Reply");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Unlikes.Unlike", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<long>("PostId");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("Unlikes");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Users.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("CreatorUserId");

                    b.Property<string>("EmailAddress");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasConversion(new ValueConverter<string, string>(v => default(string), v => default(string), new ConverterMappingHints(size: 1)));

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("ProfileImagePath");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Comments.Comment", b =>
                {
                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Posts.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.CommunityUsers.CommunityUser", b =>
                {
                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Communities.Community", "Community")
                        .WithMany("Users")
                        .HasForeignKey("CommunityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Users.User", "User")
                        .WithMany("Communities")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Likes.Like", b =>
                {
                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Posts.Post", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.PostCategories.PostCategory", b =>
                {
                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Categories.Category", "Category")
                        .WithMany("Posts")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Posts.Post", "Post")
                        .WithMany("Categories")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.PostTags.PostTag", b =>
                {
                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Posts.Post", "Post")
                        .WithMany("Tags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.PostTags.Tag", "Tag")
                        .WithMany("Posts")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Posts.Post", b =>
                {
                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Communities.Community", "Community")
                        .WithMany()
                        .HasForeignKey("CommunityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Users.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Replies.Reply", b =>
                {
                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Comments.Comment", "Comment")
                        .WithMany("Replies")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.Nnn.ApplicationCore.Entities.Unlikes.Unlike", b =>
                {
                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Posts.Post", "Post")
                        .WithMany("Unlikes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.Nnn.ApplicationCore.Entities.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
