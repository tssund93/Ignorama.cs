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
    [Migration("20181119190905_DeleteDeletedTime")]
    partial class DeleteDeletedTime
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Ignorama.Models.Post", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deleted");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<int>("ThreadID");

                    b.Property<DateTime>("Time");

                    b.Property<int>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("ThreadID");

                    b.HasIndex("UserID");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Ignorama.Models.Thread", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Deleted");

                    b.Property<bool>("Locked");

                    b.Property<bool>("Stickied");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int?>("UserID");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("Threads");
                });

            modelBuilder.Entity("Ignorama.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Ignorama.Models.Post", b =>
                {
                    b.HasOne("Ignorama.Models.Thread", "Thread")
                        .WithMany("Posts")
                        .HasForeignKey("ThreadID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Ignorama.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Ignorama.Models.Thread", b =>
                {
                    b.HasOne("Ignorama.Models.User")
                        .WithMany("Threads")
                        .HasForeignKey("UserID");
                });
#pragma warning restore 612, 618
        }
    }
}
