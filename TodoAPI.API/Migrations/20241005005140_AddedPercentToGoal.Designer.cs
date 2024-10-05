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
    [Migration("20241005005140_AddedPercentToGoal")]
    partial class AddedPercentToGoal
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("TodoAPI.Data.Models.TodoGoal", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<float>("CompletedPercent")
                        .HasColumnType("real");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdatedTime")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("TodoGoal");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            CompletedPercent = 0f,
                            CreationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Hacer un pedido de frutas y verduras frescas",
                            IsCompleted = false,
                            IsDeleted = false,
                            IsFavorite = false,
                            LastUpdatedTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Realizar Pedido a Proveedores"
                        },
                        new
                        {
                            ID = 2,
                            CompletedPercent = 0f,
                            CreationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Crear y promocionar la oferta del día para atraer más clientes",
                            IsCompleted = false,
                            IsDeleted = false,
                            IsFavorite = false,
                            LastUpdatedTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Preparar Oferta del Día"
                        });
                });

            modelBuilder.Entity("TodoAPI.Data.Models.TodoTask", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdatedTime")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("TodoTask");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            CreationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Rutina diaria de ejercicios para mantener la salud",
                            IsCompleted = false,
                            IsDeleted = false,
                            IsFavorite = false,
                            LastUpdatedTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Hacer Ejercicio"
                        },
                        new
                        {
                            ID = 2,
                            CreationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Comprar alimentos y productos necesarios para la semana",
                            IsCompleted = true,
                            IsDeleted = false,
                            IsFavorite = false,
                            LastUpdatedTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Hacer la Compra"
                        },
                        new
                        {
                            ID = 3,
                            CreationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Description = "Preparar y disfrutar de una cena ligera",
                            IsCompleted = false,
                            IsDeleted = false,
                            IsFavorite = false,
                            LastUpdatedTime = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Cenar"
                        });
                });

            modelBuilder.Entity("TodoAPI.Data.Models.TodoTaskGoal", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("TodoGoalID")
                        .HasColumnType("integer");

                    b.Property<int>("TodoTaskID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("TodoGoalID");

                    b.HasIndex("TodoTaskID");

                    b.ToTable("TodoTaskGoal");
                });

            modelBuilder.Entity("TodoAPI.Data.Models.TodoTaskGoal", b =>
                {
                    b.HasOne("TodoAPI.Data.Models.TodoGoal", "TodoGoal")
                        .WithMany("TodoTaskGoal")
                        .HasForeignKey("TodoGoalID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoAPI.Data.Models.TodoTask", "TodoTask")
                        .WithMany("TodoTaskGoal")
                        .HasForeignKey("TodoTaskID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TodoGoal");

                    b.Navigation("TodoTask");
                });

            modelBuilder.Entity("TodoAPI.Data.Models.TodoGoal", b =>
                {
                    b.Navigation("TodoTaskGoal");
                });

            modelBuilder.Entity("TodoAPI.Data.Models.TodoTask", b =>
                {
                    b.Navigation("TodoTaskGoal");
                });
#pragma warning restore 612, 618
        }
    }
}
