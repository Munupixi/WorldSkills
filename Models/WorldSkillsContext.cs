using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace WorldSkills.Models;

public partial class WorldSkillsContext : DbContext
{
    public WorldSkillsContext()
    {
    }

    public WorldSkillsContext(DbContextOptions<WorldSkillsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=123123123;database=World_Skills", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.35-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PRIMARY");

            entity.ToTable("request");

            entity.HasIndex(e => e.ExecutorId, "Executor_Id");

            entity.HasIndex(e => e.StatusId, "Status_Id");

            entity.Property(e => e.RequestId)
                .ValueGeneratedNever()
                .HasColumnName("Request_Id");
            entity.Property(e => e.Additional).HasMaxLength(100);
            entity.Property(e => e.ClientCompleteName)
                .HasMaxLength(150)
                .HasColumnName("Client_Complete_Name");
            entity.Property(e => e.ClientPhone)
                .HasMaxLength(11)
                .HasColumnName("Client_Phone");
            entity.Property(e => e.ExecutorId).HasColumnName("Executor_Id");
            entity.Property(e => e.Level).HasMaxLength(100);
            entity.Property(e => e.StatusId).HasColumnName("Status_Id");
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.Executor).WithMany(p => p.Requests)
                .HasForeignKey(d => d.ExecutorId)
                .HasConstraintName("request_ibfk_2");

            entity.HasOne(d => d.Status).WithMany(p => p.Requests)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("request_ibfk_1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.HasIndex(e => e.Title, "Title").IsUnique();

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("Role_Id");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PRIMARY");

            entity.ToTable("status");

            entity.HasIndex(e => e.Title, "Title").IsUnique();

            entity.Property(e => e.StatusId)
                .ValueGeneratedNever()
                .HasColumnName("Status_Id");
            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Login, "Login").IsUnique();

            entity.HasIndex(e => e.RoleId, "Role_Id");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("User_Id");
            entity.Property(e => e.Login).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.RoleId).HasColumnName("Role_Id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
