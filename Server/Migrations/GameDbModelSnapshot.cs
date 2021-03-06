﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server;

namespace Server.Migrations
{
    [DbContext(typeof(GameDb))]
    partial class GameDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Server.Model.CombatEncounter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CR")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("CombatEncounter");
                });

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

            modelBuilder.Entity("Server.Model.Interactable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("interactable_type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoomId");

                    b.ToTable("Interactables");

                    b.HasDiscriminator<string>("interactable_type").HasValue("Interactable");
                });

            modelBuilder.Entity("Server.Model.InteractableItem", b =>
                {
                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InteractableId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Count")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("ItemId", "InteractableId");

                    b.HasIndex("InteractableId");

                    b.ToTable("InteractionItems");
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

                    b.Property<int>("LocationId")
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

            modelBuilder.Entity("Server.Model.Skills.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Cooldown")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("skill_type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("Skills");

                    b.HasDiscriminator<string>("skill_type").HasValue("Skill");
                });

            modelBuilder.Entity("Server.Interactables.Loot", b =>
                {
                    b.HasBaseType("Server.Model.Interactable");

                    b.HasDiscriminator().HasValue("Loot");
                });

            modelBuilder.Entity("Server.Interactables.Path", b =>
                {
                    b.HasBaseType("Server.Model.Interactable");

                    b.Property<int?>("LeadsToId")
                        .HasColumnType("INTEGER");

                    b.HasIndex("LeadsToId");

                    b.HasDiscriminator().HasValue("Path");
                });

            modelBuilder.Entity("Server.Model.Items.Equipment", b =>
                {
                    b.HasBaseType("Server.Model.Items.Item");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("Equipment");
                });

            modelBuilder.Entity("Server.Model.Items.MiscItem", b =>
                {
                    b.HasBaseType("Server.Model.Items.Item");

                    b.HasDiscriminator().HasValue("MiscItem");
                });

            modelBuilder.Entity("Server.Model.Skills.DamageSkill", b =>
                {
                    b.HasBaseType("Server.Model.Skills.Skill");

                    b.Property<int>("Damage")
                        .HasColumnType("INTEGER");

                    b.HasDiscriminator().HasValue("DamageSkill");
                });

            modelBuilder.Entity("Server.Model.CombatEncounter", b =>
                {
                    b.HasOne("Server.Model.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
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

            modelBuilder.Entity("Server.Model.Interactable", b =>
                {
                    b.HasOne("Server.Model.Room", "Room")
                        .WithMany("Interactables")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Room");
                });

            modelBuilder.Entity("Server.Model.InteractableItem", b =>
                {
                    b.HasOne("Server.Model.Interactable", "Interactable")
                        .WithMany()
                        .HasForeignKey("InteractableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Model.Items.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Interactable");

                    b.Navigation("Item");
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
                    b.HasOne("Server.Model.Room", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("Server.Model.Skills.Skill", b =>
                {
                    b.HasOne("Server.Model.Items.Equipment", "Item")
                        .WithMany("Skills")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Server.Interactables.Path", b =>
                {
                    b.HasOne("Server.Model.Room", "LeadsTo")
                        .WithMany()
                        .HasForeignKey("LeadsToId");

                    b.Navigation("LeadsTo");
                });

            modelBuilder.Entity("Server.Model.Player", b =>
                {
                    b.Navigation("Equipment");

                    b.Navigation("Inventory");
                });

            modelBuilder.Entity("Server.Model.Room", b =>
                {
                    b.Navigation("Interactables");
                });

            modelBuilder.Entity("Server.Model.Items.Equipment", b =>
                {
                    b.Navigation("Skills");
                });
#pragma warning restore 612, 618
        }
    }
}
