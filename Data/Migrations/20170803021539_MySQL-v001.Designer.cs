using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CasperInc.MainSite.API.Data;
using CasperInc.MainSite.Helpers;

namespace CasperInc.MainSite.API.Data.Migrations
{
    [DbContext(typeof(MainSiteDbContext))]
    [Migration("20170803021539_MySQL-v001")]
    partial class MySQLv001
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("CasperInc.MainSite.API.Data.Models.NarrativeDataModel", b =>
                {
                    b.Property<long>("UniqueId");

                    b.Property<string>("BodyHtml")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<short>("DisplaySequence");

                    b.Property<Guid>("GuidId");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("Type");

                    b.Property<DateTime>("UpdatedOn")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("UniqueId");

                    b.HasIndex("GuidId")
                        .IsUnique();

                    b.ToTable("NarrativeData");

                    b.HasDiscriminator<string>("Discriminator").HasValue("NarrativeDataModel");
                });

            modelBuilder.Entity("CasperInc.MainSite.API.Data.Models.NarrativeTagDataModel", b =>
                {
                    b.Property<long>("NarrativeId");

                    b.Property<long>("TagId");

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("UniqueId");

                    b.Property<DateTime>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("NarrativeId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("NarrativeTagCrossWalk");
                });

            modelBuilder.Entity("CasperInc.MainSite.API.Data.Models.TagDataModel", b =>
                {
                    b.Property<long>("UniqueId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("GuidId");

                    b.Property<string>("KeyWord")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("UniqueId");

                    b.HasIndex("GuidId")
                        .IsUnique();

                    b.HasIndex("KeyWord")
                        .IsUnique();

                    b.ToTable("TagData");
                });

            modelBuilder.Entity("CasperInc.MainSite.API.Data.Models.UserDataModel", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedDate")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DisplayName");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<DateTime>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CasperInc.MainSite.API.Data.Models.CommentDataModel", b =>
                {
                    b.HasBaseType("CasperInc.MainSite.API.Data.Models.NarrativeDataModel");

                    b.Property<long?>("NarrativeDataModelUniqueId");

                    b.Property<long?>("ParentId");

                    b.HasIndex("NarrativeDataModelUniqueId");

                    b.HasIndex("ParentId");

                    b.ToTable("CommentDataModel");

                    b.HasDiscriminator().HasValue("CommentDataModel");
                });

            modelBuilder.Entity("CasperInc.MainSite.API.Data.Models.NarrativeTagDataModel", b =>
                {
                    b.HasOne("CasperInc.MainSite.API.Data.Models.NarrativeDataModel", "NarrativeData")
                        .WithMany("NarrativeTags")
                        .HasForeignKey("NarrativeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CasperInc.MainSite.API.Data.Models.TagDataModel", "TagData")
                        .WithMany("NarrativeTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CasperInc.MainSite.API.Data.Models.UserDataModel")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CasperInc.MainSite.API.Data.Models.UserDataModel")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CasperInc.MainSite.API.Data.Models.UserDataModel")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CasperInc.MainSite.API.Data.Models.CommentDataModel", b =>
                {
                    b.HasOne("CasperInc.MainSite.API.Data.Models.NarrativeDataModel")
                        .WithMany("comments")
                        .HasForeignKey("NarrativeDataModelUniqueId");

                    b.HasOne("CasperInc.MainSite.API.Data.Models.CommentDataModel", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");
                });
        }
    }
}
