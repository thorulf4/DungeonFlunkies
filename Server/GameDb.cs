using Microsoft.EntityFrameworkCore;
using Server.Model;
using Server.Model.Items;
using Server.Model.Skills;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Server
{
    public class GameDb : DbContext, ISavable
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Interactable> Interactables { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<OwnedBy> OwnedBys { get; set; }
        public DbSet<InteractableItem> InteractionItems { get; set; }
        public DbSet<Equipped> Equipped { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Filename=main.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasOne<Room>("Location").WithMany().HasForeignKey("LocationId").OnDelete(DeleteBehavior.Restrict);

            //Get all Interactables in assembly and make it TPH polymorphic
            var disc = modelBuilder.Entity<Interactable>()
                .HasDiscriminator<string>("interactable_type");
            modelBuilder.Entity<Interactable>()
                .HasOne<Room>("Room").WithMany("Interactables").HasForeignKey("RoomId").IsRequired();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(Interactable)).ToArray())
            {
                disc.HasValue(type, type.Name);
            }

            disc = modelBuilder.Entity<Item>()
                .HasDiscriminator<string>("item_type");

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Item))).ToArray())
            {
                disc.HasValue(type, type.Name);
            }


            disc = modelBuilder.Entity<Skill>()
                .HasDiscriminator<string>("skill_type");
            modelBuilder.Entity<Skill>().HasOne<Equipment>("Item").WithMany("Skills").HasForeignKey("ItemId").IsRequired().OnDelete(DeleteBehavior.Cascade);

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Skill))).ToArray())
            {
                disc.HasValue(type, type.Name);
            }

            modelBuilder.Entity<OwnedBy>().HasKey("OwnsId", "OwnerId");
            modelBuilder.Entity<OwnedBy>().HasOne<Player>("Owner").WithMany("Inventory").HasForeignKey("OwnerId").IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OwnedBy>().HasOne<Item>("Owns").WithMany().HasForeignKey("OwnsId").IsRequired().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InteractableItem>().HasKey("ItemId", "InteractableId");
            modelBuilder.Entity<InteractableItem>().HasOne<Interactable>("Interactable").WithMany().HasForeignKey("InteractableId").IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<InteractableItem>().HasOne<Item>("Item").WithMany().HasForeignKey("ItemId").IsRequired().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Equipped>().HasOne<Item>("Item").WithMany().HasForeignKey("ItemId").IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Equipped>().HasOne<Player>("Player").WithMany("Equipment").HasForeignKey("PlayerId").IsRequired().OnDelete(DeleteBehavior.Cascade);
        }

        void ISavable.Save() => SaveChanges();
    }
}
