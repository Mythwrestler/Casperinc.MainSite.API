using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CasperInc.MainSiteCore.Data;

namespace MainSiteCore.Data.Migrations
{
    [DbContext(typeof(MainSiteCoreDBContext))]
    partial class MainSiteCoreDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("CasperInc.MainSiteCore.Data.Models.NarrativeDataModel", b =>
                {
                    b.Property<int>("Id")
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

            modelBuilder.Entity("CasperInc.MainSiteCore.Data.Models.NarrativeTagDataModel", b =>
                {
                    b.Property<int>("NarrativeId");

                    b.Property<string>("TagKeyWord");

                    b.HasKey("NarrativeId", "TagKeyWord");

                    b.HasIndex("TagKeyWord");

                    b.ToTable("NarrativeTagDataModel");
                });

            modelBuilder.Entity("CasperInc.MainSiteCore.Data.Models.TagDataModel", b =>
                {
                    b.Property<string>("KeyWord")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("KeyWord");

                    b.ToTable("TagData");
                });

            modelBuilder.Entity("CasperInc.MainSiteCore.Data.Models.NarrativeTagDataModel", b =>
                {
                    b.HasOne("CasperInc.MainSiteCore.Data.Models.NarrativeDataModel", "NarrativeData")
                        .WithMany("NarrativeTags")
                        .HasForeignKey("NarrativeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CasperInc.MainSiteCore.Data.Models.TagDataModel", "TagData")
                        .WithMany("NarrativeTags")
                        .HasForeignKey("TagKeyWord")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
