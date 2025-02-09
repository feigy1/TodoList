using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace TodoList;

public partial class ToDoDbContext : DbContext
{
    public ToDoDbContext()
    {
    }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Session> Sessions { get; set; }

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseMySql(
        "server=localhost;user=feigy;password=PHHDH;database=tasks", 
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.0-mysql"));


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("items");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("Name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PRIMARY");

            entity.ToTable("users");

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

        entity.ToTable("sessions");

        entity.HasIndex(e => e.UserId, "user_id");

        entity.Property(e => e.Date)
             .HasDefaultValueSql("CURRENT_TIMESTAMP")
             .HasColumnType("timestamp")
             .HasColumnName("date");

        entity.Property(e => e.UserId)
        .HasColumnName("user_id");

        entity.HasOne(d => d.User)
        .WithMany()
        .HasForeignKey(d => d.UserId)
        .OnDelete(DeleteBehavior.ClientSetNull)
        .HasConstraintName("sessions_ibfk_1");     
    });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}


// using System;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;
// using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

// namespace TodoList;

// public partial class ToDoDbContext : DbContext
// {
//     public ToDoDbContext()
//     {
//     }

//     public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
//         : base(options)
//     {
//     }

//     public virtual DbSet<Item> Items { get; set; }

//     public virtual DbSet<Session> Sessions { get; set; }

//     public virtual DbSet<User> Users { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         => optionsBuilder.UseMySql("name=ToDoDB", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.40-mysql"));

//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         modelBuilder
//             .UseCollation("utf8mb4_0900_ai_ci")
//             .HasCharSet("utf8mb4");

//         modelBuilder.Entity<Item>(entity =>
//         {
//             entity.HasKey(e => e.Id).HasName("PRIMARY");

//             entity.ToTable("items");

//             entity.Property(e => e.Id).HasColumnName("Id");
//             entity.Property(e => e.Name)
//                 .HasMaxLength(100)
//                 .HasColumnName("Name");
//         });

// modelBuilder.Entity<Session>(entity =>
// {
//     // הגדרת המפתח הראשי
//     entity.HasKey(e => e.Number).HasName("PRIMARY");  // זהו השדה החדש 'number' שמוגדר כמפתח ראשי

//     entity.ToTable("sessions");

//     // הגדרת האינדקס על השדה UserId
//     entity.HasIndex(e => e.UserId, "user_id");

//     // הגדרת שדה ה-Date
//     entity.Property(e => e.Date)
//         .HasDefaultValueSql("CURRENT_TIMESTAMP")
//         .HasColumnType("timestamp")
//         .HasColumnName("date");

//     // הגדרת שדה ה-UserId
//     entity.Property(e => e.UserId)
//         .HasColumnName("user_id");

//     // הגדרת הקשר עם הטבלה Users
//     entity.HasOne(d => d.User)
//         .WithMany()
//         .HasForeignKey(d => d.UserId)
//         .OnDelete(DeleteBehavior.ClientSetNull)
//         .HasConstraintName("sessions_ibfk_1");
// });
//         modelBuilder.Entity<User>(entity =>
//         {
//             entity.HasKey(e => e.id).HasName("PRIMARY");

//             entity.ToTable("users");

//             entity.HasIndex(e => e.password, "password").IsUnique();

//             entity.Property(e => e.id).HasColumnName("id");
//             entity.Property(e => e.password).HasColumnName("password");
//             entity.Property(e => e.username)
//                 .HasMaxLength(255)
//                 .HasColumnName("username");
//         });

//         OnModelCreatingPartial(modelBuilder);
//     }

//     partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
// }

