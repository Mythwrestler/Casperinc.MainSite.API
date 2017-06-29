using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using Casperinc.MainSite.API.Data.Models;

namespace Casperinc.MainSite.API.Data
{

    public class MainSiteDbContext : DbContext
    {

        public MainSiteDbContext(DbContextOptions options) : base(options)
        { 
            Database.Migrate();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

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

        }

        public DbSet<NarrativeDataModel> NarrativeData { get; set; }
        public DbSet<TagDataModel> TagData { get; set; }
        public DbSet<NarrativeTagDataModel> NarrativeTagCrossWalk { get; set; }

    }


}