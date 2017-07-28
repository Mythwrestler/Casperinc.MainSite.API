using Microsoft.EntityFrameworkCore;

using CasperInc.MainSite.API.Data.Models;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CasperInc.MainSite.API.Data
{

    public class MainSiteDbContext : IdentityDbContext<UserDataModel>
    {

        public MainSiteDbContext(DbContextOptions options) : base(options)
        {
            Database.Migrate();
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

            modelBuilder.Entity<NarrativeUserDataModel>()
                .HasKey(nu => new { nu.NarrativeId, nu.UserId});

            modelBuilder.Entity<NarrativeUserDataModel>()
                .HasOne(nu => nu.NarrativeData)
                .WithMany(n => n.Authors)
                .HasForeignKey(nu => nu.NarrativeId);

            modelBuilder.Entity<NarrativeUserDataModel>()
                .HasOne(nu => nu.UserData)
                .WithMany(u => u.Narratives)
                .HasForeignKey(nu => nu.UserId);
        }

        public DbSet<NarrativeDataModel> NarrativeData { get; set; }
        public DbSet<TagDataModel> TagData { get; set; }
        public DbSet<NarrativeTagDataModel> NarrativeTagCrossWalk { get; set; }
        public DbSet<NarrativeUserDataModel> AuthorCrossWalk { get; set; }

        public DbSet<CommentDataModel> CommentData { get; set; }

    }


}