using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace API;

public partial class ToDoDbContext : DbContext
{
    public ToDoDbContext()
    {
    }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Items> Items { get; set; }
    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<Session> Sessions { get; set; }

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseMySql(
        "server=bc8ooc32y4vwockv2xmw-mysql.services.clever-cloud.com;user=uf2ezku5kqwyw0mn;password=QRaZTDFqyCOad4JxiS7J;database=bc8ooc32y4vwockv2xmw", 
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.0-mysql"));


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Items>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Items");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("Name");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("Users");

            entity.HasIndex(e => e.password, "password").IsUnique();

            entity.Property(e => e.id).HasColumnName("id");
            entity.Property(e => e.password).HasColumnName("password");
            entity.Property(e => e.username)
                .HasMaxLength(255)
                .HasColumnName("username");
        });

modelBuilder.Entity<Session>(entity =>
    {
        entity.HasKey(e => e.Number).HasName("PRIMARY");

        entity.ToTable("Sessions");

        entity.HasIndex(e => e.User_id, "user_id");

        entity.Property(e => e.Date)
             .HasDefaultValueSql("CURRENT_TIMESTAMP")
             .HasColumnType("timestamp")
             .HasColumnName("date");

        entity.Property(e => e.User_id)
        .HasColumnName("user_id");

        entity.HasOne(d => d.User)
        .WithMany()
        .HasForeignKey(d => d.User_id)
        .OnDelete(DeleteBehavior.ClientSetNull)
        .HasConstraintName("sessions_ibfk_1");     
    });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
