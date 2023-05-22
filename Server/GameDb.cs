using Microsoft.EntityFrameworkCore;
using Server.Application.Combat.Skills;
using Server.Interactables;
using Server.Model;
using Server.Model.Items;
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
        public DbSet<Item> Items { get; set; }
        public DbSet<OwnedBy> OwnedBys { get; set; }
        public DbSet<Equipped> Equipped { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Filename=main.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var disc = modelBuilder.Entity<Item>()
                .HasDiscriminator<string>("item_type");

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(Item))).ToArray())
            {
                disc.HasValue(type, type.Name);
            }

            modelBuilder.Entity<OwnedBy>().HasKey("OwnsId", "OwnerId");
            modelBuilder.Entity<OwnedBy>().HasOne<Player>("Owner").WithMany("Inventory").HasForeignKey("OwnerId").IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OwnedBy>().HasOne<Item>("Owns").WithMany().HasForeignKey("OwnsId").IsRequired().OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Equipped>().HasOne<Item>("Item").WithMany().HasForeignKey("ItemId").IsRequired().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Equipped>().HasOne<Player>("Player").WithMany("Equipment").HasForeignKey("PlayerId").IsRequired().OnDelete(DeleteBehavior.Cascade);
        }

        void ISavable.Save() => SaveChanges();
    }
}
