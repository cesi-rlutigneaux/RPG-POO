using Microsoft.EntityFrameworkCore;
using RPG_POO.Models.History;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_POO.Data
{
    public sealed class SaveDbContext : DbContext
    {
        public DbSet<HeroSaveEntity> HeroSaves => Set<HeroSaveEntity>();
        public DbSet<ItemSaveEntity> Items => Set<ItemSaveEntity>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(
                Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..")),
                "rpg_save.db"
            );
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HeroSaveEntity>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<HeroSaveEntity>()
                .HasMany(x => x.Items)
                .WithOne(x => x.HeroSave!)
                .HasForeignKey(x => x.HeroSaveEntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
