using Microsoft.EntityFrameworkCore;
using Server.Model;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Server
{
    public class GameDb : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Interactable> Interactables { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Filename=main.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Get all Interactables in assembly and make it TPH polymorphic
            var disc = modelBuilder.Entity<Interactable>()
                .HasDiscriminator<string>("type");
            modelBuilder.Entity<Interactable>()
                .HasOne<Room>("Room").WithMany("Interactables").HasForeignKey("RoomId").IsRequired();

            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.BaseType == typeof(Interactable)).ToArray())
            {
                disc.HasValue(type, type.Name);
            }
        }
    }
}
