using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Casperinc.MainSite.API.Data;

namespace CasperInc.MainSite.API.Data.Migrations
{
    [DbContext(typeof(MainSiteDbContext))]
    [Migration("20170702175545_MySQLVersion001")]
    partial class MySQLVersion001
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Casperinc.MainSite.API.Data.Models.NarrativeDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BodyHtml")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedOn");

                    b.HasKey("Id");

                    b.ToTable("NarrativeData");
                });

            modelBuilder.Entity("Casperinc.MainSite.API.Data.Models.NarrativeTagDataModel", b =>
                {
                    b.Property<Guid>("NarrativeId");

                    b.Property<Guid>("TagId");

                    b.HasKey("NarrativeId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("NarrativeTagCrossWalk");
                });

            modelBuilder.Entity("Casperinc.MainSite.API.Data.Models.TagDataModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("KeyWord")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("KeyWord")
                        .IsUnique();

                    b.ToTable("TagData");
                });

            modelBuilder.Entity("Casperinc.MainSite.API.Data.Models.NarrativeTagDataModel", b =>
                {
                    b.HasOne("Casperinc.MainSite.API.Data.Models.NarrativeDataModel", "NarrativeData")
                        .WithMany("NarrativeTags")
                        .HasForeignKey("NarrativeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Casperinc.MainSite.API.Data.Models.TagDataModel", "TagData")
                        .WithMany("NarrativeTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
