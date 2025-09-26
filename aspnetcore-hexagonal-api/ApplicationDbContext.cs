using aspnetcore_hexagonal_api.Features.FeatureExample.Application;
using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Models;
using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_hexagonal_api;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<FeatureExampleEntity> FeatureExamples { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<FeatureExampleEntity>(entity =>
        {
            entity.ToTable(FeatureExampleConstants.TableName);

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .IsRequired(false);

            entity.Property(e => e.CreatedBy)
                .HasMaxLength(100);

            entity.Property(e => e.UpdatedBy)
                .HasMaxLength(100);

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.HasIndex(e => e.Name);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.IsActive);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FeatureExampleEntity>().HasData(
            new FeatureExampleEntity
            {
                Id = 1,
                Name = "Exemplo Inicial",
                Description = "Este é um exemplo inicial para demonstrar a funcionalidade",
                Status = FeatureExampleStatus.Active,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = "System",
                IsActive = true
            },
            new FeatureExampleEntity
            {
                Id = 2,
                Name = "Exemplo Pendente",
                Description = "Este exemplo está em status pendente",
                Status = FeatureExampleStatus.Pending,
                CreatedAt = new DateTime(2024, 1, 2, 0, 0, 0, DateTimeKind.Utc),
                CreatedBy = "System",
                IsActive = true
            }
        );
    }
}