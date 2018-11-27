﻿// <auto-generated />
using System;
using Ignorama.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ignorama.Migrations
{
    [DbContext(typeof(ForumContext))]
    [Migration("20181127045341_AddAnnouncementsTag")]
    partial class AddAnnouncementsTag
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ignorama.Models.FollowedThread", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IP");

                    b.Property<int?>("LastSeenPostID");

                    b.Property<int>("ThreadID");

                    b.Property<long?>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("LastSeenPostID");

                    b.HasIndex("ThreadID");

                    b.HasIndex("UserId");

                    b.ToTable("FollowedThreads");
                });

            modelBuilder.Entity("Ignorama.Models.HiddenThread", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IP");

                    b.Property<int>("ThreadID");

                    b.Property<long?>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("ThreadID");

                    b.HasIndex("UserId");

                    b.ToTable("HiddenThreads");
                });

            modelBuilder.Entity("Ignorama.Models.PermissionLevel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Level");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("PermissionLevels");
                });

            modelBuilder.Entity("Ignorama.Models.Post", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Anonymous");

                    b.Property<bool>("Bump");

                    b.Property<bool>("Deleted");

                    b.Property<string>("IP");

                    b.Property<bool>("RevealOP");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<int>("ThreadID");

                    b.Property<DateTime>("Time");

                    b.Property<long?>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("ThreadID");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Ignorama.Models.SelectedTag", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IP");

                    b.Property<int>("TagID");

                    b.Property<long?>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("TagID");

                    b.HasIndex("UserId");

                    b.ToTable("SelectedTags");
                });

            modelBuilder.Entity("Ignorama.Models.Tag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AlwaysVisible");

                    b.Property<bool>("Deleted");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<long?>("ReadRoleId");

                    b.Property<long?>("WriteRoleId");

                    b.HasKey("ID");

                    b.HasIndex("ReadRoleId");

                    b.HasIndex("WriteRoleId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Ignorama.Models.Thread", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deleted");

                    b.Property<bool>("Locked");

                    b.Property<bool>("Stickied");

                    b.Property<int>("TagID");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<long?>("UserId");

                    b.HasKey("ID");

                    b.HasIndex("TagID");

                    b.HasIndex("UserId");

                    b.ToTable("Threads");
                });

            modelBuilder.Entity("Ignorama.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

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

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<long>", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<long>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Ignorama.Models.FollowedThread", b =>
                {
                    b.HasOne("Ignorama.Models.Post", "LastSeenPost")
                        .WithMany()
                        .HasForeignKey("LastSeenPostID");

                    b.HasOne("Ignorama.Models.Thread", "Thread")
                        .WithMany()
                        .HasForeignKey("ThreadID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ignorama.Models.User", "User")
                        .WithMany("FollowedThreads")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Ignorama.Models.HiddenThread", b =>
                {
                    b.HasOne("Ignorama.Models.Thread", "Thread")
                        .WithMany()
                        .HasForeignKey("ThreadID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ignorama.Models.User", "User")
                        .WithMany("HiddenThreads")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Ignorama.Models.Post", b =>
                {
                    b.HasOne("Ignorama.Models.Thread", "Thread")
                        .WithMany("Posts")
                        .HasForeignKey("ThreadID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ignorama.Models.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Ignorama.Models.SelectedTag", b =>
                {
                    b.HasOne("Ignorama.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ignorama.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Ignorama.Models.Tag", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<long>", "ReadRole")
                        .WithMany()
                        .HasForeignKey("ReadRoleId");

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<long>", "WriteRole")
                        .WithMany()
                        .HasForeignKey("WriteRoleId");
                });

            modelBuilder.Entity("Ignorama.Models.Thread", b =>
                {
                    b.HasOne("Ignorama.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ignorama.Models.User")
                        .WithMany("Threads")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<long>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<long>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<long>", b =>
                {
                    b.HasOne("Ignorama.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<long>", b =>
                {
                    b.HasOne("Ignorama.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<long>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ignorama.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<long>", b =>
                {
                    b.HasOne("Ignorama.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
