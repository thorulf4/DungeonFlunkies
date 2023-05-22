﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(GameDb))]
    [Migration("20230522200653_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.6");

            modelBuilder.Entity("Server.Model.Equipped", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EquipmentType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Slot")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("PlayerId");

                    b.ToTable("Equipped");
                });

            modelBuilder.Entity("Server.Model.Items.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaseValue")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("item_type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Items");

                    b.HasDiscriminator<string>("item_type").HasValue("Item");
                });

            modelBuilder.Entity("Server.Model.OwnedBy", b =>
                {
                    b.Property<int>("OwnsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("OwnsId", "OwnerId");

                    b.HasIndex("OwnerId");

                    b.ToTable("OwnedBys");
                });

            modelBuilder.Entity("Server.Model.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Health")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Secret")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Server.Model.Items.Equipment", b =>
                {
                    b.HasBaseType("Server.Model.Items.Item");

                    b.Property<string>("EquipmentTemplate")
                        .HasColumnType("TEXT");

                    b.Property<int>("ItemPower")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Equipment");
                });

            modelBuilder.Entity("Server.Model.Items.MiscItem", b =>
                {
                    b.HasBaseType("Server.Model.Items.Item");

                    b.HasDiscriminator().HasValue("MiscItem");
                });

            modelBuilder.Entity("Server.Model.Equipped", b =>
                {
                    b.HasOne("Server.Model.Items.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Model.Player", "Player")
                        .WithMany("Equipment")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Server.Model.OwnedBy", b =>
                {
                    b.HasOne("Server.Model.Player", "Owner")
                        .WithMany("Inventory")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Model.Items.Item", "Owns")
                        .WithMany()
                        .HasForeignKey("OwnsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Owns");
                });

            modelBuilder.Entity("Server.Model.Player", b =>
                {
                    b.Navigation("Equipment");

                    b.Navigation("Inventory");
                });
#pragma warning restore 612, 618
        }
    }
}