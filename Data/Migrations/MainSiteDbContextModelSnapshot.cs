﻿// <auto-generated />
using Casperinc.MainSite.API.Data;
using Casperinc.MainSite.API.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Casperinc.MainSite.API.Data.Migrations
{
    [DbContext(typeof(MainSiteDbContext))]
    partial class MainSiteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("Casperinc.MainSite.API.Data.Models.CommentDataModel", b =>
                {
                    b.Property<long>("UniqueId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("GuidId");

                    b.Property<long>("NarrativeId");

                    b.Property<long?>("ParentId");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedOn")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("UniqueId");

                    b.HasIndex("GuidId")
                        .IsUnique();

                    b.HasIndex("NarrativeId");

                    b.HasIndex("ParentId");

                    b.ToTable("CommentData");
                });

            modelBuilder.Entity("Casperinc.MainSite.API.Data.Models.NarrativeDataModel", b =>
                {
                    b.Property<long>("UniqueId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BodyHtml")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<short?>("DisplaySequence");

                    b.Property<Guid>("GuidId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("Type");

                    b.Property<DateTime>("UpdatedOn")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("UniqueId");

                    b.HasIndex("GuidId")
                        .IsUnique();

                    b.ToTable("NarrativeData");
                });

            modelBuilder.Entity("Casperinc.MainSite.API.Data.Models.NarrativeTagDataModel", b =>
                {
                    b.Property<long>("NarrativeId");

                    b.Property<long>("TagId");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

                    b.HasKey("NarrativeId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("NarrativeTagCrossWalk");
                });

            modelBuilder.Entity("Casperinc.MainSite.API.Data.Models.TagDataModel", b =>
                {
                    b.Property<long>("UniqueId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("GuidId");

                    b.Property<string>("KeyWord")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn);

                    b.HasKey("UniqueId");

                    b.HasIndex("GuidId")
                        .IsUnique();

                    b.HasIndex("KeyWord")
                        .IsUnique();

                    b.ToTable("TagData");
                });

            modelBuilder.Entity("Casperinc.MainSite.API.Data.Models.CommentDataModel", b =>
                {
                    b.HasOne("Casperinc.MainSite.API.Data.Models.NarrativeDataModel", "Narrative")
                        .WithMany("Comments")
                        .HasForeignKey("NarrativeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Casperinc.MainSite.API.Data.Models.CommentDataModel", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");
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
#pragma warning restore 612, 618
        }
    }
}
