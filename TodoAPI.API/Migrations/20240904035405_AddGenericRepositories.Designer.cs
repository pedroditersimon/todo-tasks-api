﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TodoAPI.API.Services;


#nullable disable

namespace TodoAPI.API.Migrations
{
    [DbContext(typeof(TodoDBContext))]
    [Migration("20240904035405_AddGenericRepositories")]
    partial class AddGenericRepositories
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TodoAPI.Data.Models.TodoGoal", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("TodoGoal");
                });

            modelBuilder.Entity("TodoAPI.Data.Models.TodoTask", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<bool>("Completed")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int?>("TodoGoalID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("TodoGoalID");

                    b.ToTable("TodoTask");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Completed = false,
                            CreationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Inserted by ef migrations",
                            IsDeleted = false,
                            Name = "my First Task"
                        },
                        new
                        {
                            ID = 2,
                            Completed = false,
                            CreationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "ModelBuilder builder",
                            IsDeleted = false,
                            Name = "TodoTask1"
                        });
                });

            modelBuilder.Entity("TodoAPI.Data.Models.TodoTask", b =>
                {
                    b.HasOne("TodoAPI.Data.Models.TodoGoal", null)
                        .WithMany("Tasks")
                        .HasForeignKey("TodoGoalID");
                });

            modelBuilder.Entity("TodoAPI.Data.Models.TodoGoal", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
