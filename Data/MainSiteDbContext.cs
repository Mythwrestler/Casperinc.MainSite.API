using Microsoft.EntityFrameworkCore;

using Casperinc.MainSite.API.Data.Models;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Casperinc.MainSite.API.Data
{

    public class MainSiteDbContext : DbContext
    {

        public MainSiteDbContext(DbContextOptions options) : base(options)
        {
            //Database.Migrate();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NarrativeDataModel>()
                        .HasIndex(n => n.GuidId)
                        .IsUnique();

            modelBuilder.Entity<TagDataModel>()
                        .HasIndex(t => t.GuidId)
                        .IsUnique();

            modelBuilder.Entity<CommentDataModel>()
                        .HasIndex(c => c.GuidId)
                        .IsUnique();

            // modelBuilder.Entity<CommentDataModel>()
            //             .HasOne(c => c.Author)
            //             .WithMany(u => u.Comments)
            //             .HasForeignKey(c => c.UserId)
            //             .OnDelete(DeleteBehavior.Restrict);
                        
            modelBuilder.Entity<CommentDataModel>()
                        .HasOne(c => c.Narrative)
                        .WithMany(i => i.Comments);

            modelBuilder.Entity<CommentDataModel>()
                        .HasOne(c => c.Parent)
                        .WithMany(c => c.Children);

            modelBuilder.Entity<CommentDataModel>()
                        .HasMany(c => c.Children)
                        .WithOne(c => c.Parent);   

            // modelBuilder.Entity<NarrativeDataModel>()
            //             .HasOne(i => i.Author)
            //             .WithMany(u => u.Narratives);

            modelBuilder.Entity<NarrativeDataModel>()
                        .HasMany(i => i.Comments)
                        .WithOne(c => c.Narrative);

            // modelBuilder.Entity<UserDataModel>()
            //             .HasMany(u => u.Narratives)
            //             .WithOne(i => i.Author);

            // modelBuilder.Entity<UserDataModel>()
            //             .HasMany(u => u.Comments)
            //             .WithOne(c => c.Author)
            //             .HasPrincipalKey(u => u.Id);

            modelBuilder.Entity<TagDataModel>()
                        .HasIndex(t => t.KeyWord)
                        .IsUnique();
            
            modelBuilder.Entity<NarrativeTagDataModel>()
                .HasKey(t => new { t.NarrativeId, t.TagId });

            modelBuilder.Entity<NarrativeTagDataModel>()
                .HasOne(pt => pt.NarrativeData)
                .WithMany(p => p.NarrativeTags)
                .HasForeignKey(pt => pt.NarrativeId);

            modelBuilder.Entity<NarrativeTagDataModel>()
                .HasOne(pt => pt.TagData)
                .WithMany(t => t.NarrativeTags)
                .HasForeignKey(pt => pt.TagId);

            // modelBuilder.Entity<NarrativeUserDataModel>()
            //     .HasKey(nu => new { nu.NarrativeId, nu.UserId});

            // modelBuilder.Entity<NarrativeUserDataModel>()
            //     .HasOne(nu => nu.NarrativeData)
            //     .WithMany(n => n.Authors)
            //     .HasForeignKey(nu => nu.NarrativeId);

            // modelBuilder.Entity<NarrativeUserDataModel>()
            //     .HasOne(nu => nu.UserData)
            //     .WithMany(u => u.Narratives)
            //     .HasForeignKey(nu => nu.UserId);
        }

        public DbSet<NarrativeDataModel> NarrativeData { get; set; }
        public DbSet<TagDataModel> TagData { get; set; }
        public DbSet<NarrativeTagDataModel> NarrativeTagCrossWalk { get; set; }
        public DbSet<CommentDataModel> CommentData { get; set; }

    }


}