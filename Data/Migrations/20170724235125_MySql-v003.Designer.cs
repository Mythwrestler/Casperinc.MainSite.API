using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Casperinc.MainSite.API.Data;

namespace CasperInc.MainSite.API.Data.Migrations
{
    [DbContext(typeof(MainSiteDbContext))]
    [Migration("20170724235125_MySql-v003")]
    partial class MySqlv003
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Casperinc.MainSite.API.Data.Models.NarrativeDataModel", b =>
                {
                    b.Property<long>("UniqueId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BodyHtml")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<short>("DisplaySequence");

                    b.Property<Guid>("Id");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedOn")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("UniqueId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("NarrativeData");
                });

            modelBuilder.Entity("Casperinc.MainSite.API.Data.Models.NarrativeTagDataModel", b =>
                {
                    b.Property<long>("NarrativeId");

                    b.Property<long>("TagId");

                    b.HasKey("NarrativeId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("NarrativeTagCrossWalk");
                });

            modelBuilder.Entity("Casperinc.MainSite.API.Data.Models.TagDataModel", b =>
                {
                    b.Property<long>("UniqueId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("Id");

                    b.Property<string>("KeyWord")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("UniqueId");

                    b.HasIndex("Id")
                        .IsUnique();

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
