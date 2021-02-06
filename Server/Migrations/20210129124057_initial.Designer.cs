﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server;

namespace Server.Migrations
{
    [DbContext(typeof(GameDb))]
    [Migration("20210129124057_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Server.Model.Interactable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Interactables");

                    b.HasDiscriminator<string>("type").HasValue("Interactable");
                });

            modelBuilder.Entity("Server.Model.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Secret")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Server.Model.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Server.Interactables.Path", b =>
                {
                    b.HasBaseType("Server.Model.Interactable");

                    b.Property<int?>("LeadsToId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("LeadsToId");

                    b.HasDiscriminator().HasValue("Path");
                });

            modelBuilder.Entity("Server.Model.Interactable", b =>
                {
                    b.HasOne("Server.Model.Room", "Room")
                        .WithMany("Interactables")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Server.Model.Player", b =>
                {
                    b.HasOne("Server.Model.Room", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Server.Interactables.Path", b =>
                {
                    b.HasOne("Server.Model.Room", "LeadsTo")
                        .WithMany()
                        .HasForeignKey("LeadsToId");

                    b.Navigation("LeadsTo");
                });

            modelBuilder.Entity("Server.Model.Room", b =>
                {
                    b.Navigation("Interactables");
                });
#pragma warning restore 612, 618
        }
    }
}