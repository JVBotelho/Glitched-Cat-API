﻿using GlitchedCat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GlitchedCat.Infra.Data
{
    public class BlogContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=localhost\\SQLEXPRESS;Database=BlogDb;Trusted_Connection=true;Encrypt=False;");
            }
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            modelBuilder.Entity<Post>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Comment>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<User>()
                .HasKey(c => c.Id);
        }
    }
}