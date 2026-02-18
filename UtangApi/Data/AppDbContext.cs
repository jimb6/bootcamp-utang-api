using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UtangApp.Api.Entities;
using UtangApp.Api.Enums;

namespace UtangApp.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Borrower> Borrowers => Set<Borrower>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Offer> Offers => Set<Offer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var contractStatusConverter = new ValueConverter<ContractStatus, string>(
            v => v.ToString().ToLowerInvariant(),
            v => Enum.Parse<ContractStatus>(v, true));

        var offerStatusConverter = new ValueConverter<OfferStatus, string>(
            v => v.ToString().ToLowerInvariant(),
            v => Enum.Parse<OfferStatus>(v, true));

        var termTypeConverter = new ValueConverter<TermType, string>(
            v => v.ToString().ToLowerInvariant(),
            v => Enum.Parse<TermType>(v, true));

        var interestModeConverter = new ValueConverter<InterestMode, string>(
            v => v.ToString().ToLowerInvariant(),
            v => Enum.Parse<InterestMode>(v, true));

        // Borrower
        modelBuilder.Entity<Borrower>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.EmergencyContactName).HasMaxLength(200);
            entity.Property(e => e.EmergencyContactPhone).HasMaxLength(20);
            entity.Ignore(e => e.FullName);
        });

        // Contract
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PrincipalAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5,2)");
            entity.Property(e => e.LiquidationRate).HasColumnType("decimal(5,2)").HasDefaultValue(0m);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.RemainingBalance).HasColumnType("decimal(18,2)");
            entity.Property(e => e.AmountPerTerm).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.InterestMode).HasConversion(interestModeConverter).HasMaxLength(20);
            entity.Property(e => e.TermType).HasConversion(termTypeConverter).HasMaxLength(20);
            entity.Property(e => e.Status).HasConversion(contractStatusConverter).HasMaxLength(20);

            entity.HasOne(e => e.Borrower)
                  .WithMany(b => b.Contracts)
                  .HasForeignKey(e => e.BorrowerId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Payments)
                  .WithOne(p => p.Contract)
                  .HasForeignKey(p => p.ContractId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.ReceiptNumber).HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(500);
        });

        // Offer
        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OfferedAmount).HasColumnType("decimal(18,2)");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5,2)");
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.Status).HasConversion(offerStatusConverter).HasMaxLength(20);

            entity.HasOne(e => e.Borrower)
                  .WithMany(b => b.Offers)
                  .HasForeignKey(e => e.BorrowerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2026, 1, 15, 8, 30, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Borrower>().HasData(
            new Borrower
            {
                Id = 1,
                FirstName = "Juan",
                LastName = "Dela Cruz",
                BirthDate = new DateOnly(1990, 5, 15),
                Email = "juan@email.com",
                Phone = "09171234567",
                Address = "Cebu City",
                EmergencyContactName = "Maria",
                EmergencyContactPhone = "09179876543",
                CreatedAt = now,
                UpdatedAt = now
            },
            new Borrower
            {
                Id = 2,
                FirstName = "Maria",
                LastName = "Santos",
                BirthDate = new DateOnly(1988, 3, 22),
                Email = "maria@email.com",
                Phone = "09181234567",
                Address = "Manila",
                EmergencyContactName = "Jose",
                EmergencyContactPhone = "09189876543",
                CreatedAt = now,
                UpdatedAt = now
            },
            new Borrower
            {
                Id = 3,
                FirstName = "Pedro",
                LastName = "Reyes",
                BirthDate = new DateOnly(1995, 11, 8),
                Email = "pedro@email.com",
                Phone = "09191234567",
                Address = "Davao City",
                CreatedAt = now,
                UpdatedAt = now
            });

        modelBuilder.Entity<Contract>().HasData(
            new Contract
            {
                Id = 1,
                BorrowerId = 1,
                PrincipalAmount = 10000m,
                InterestRate = 5m,
                InterestMode = InterestMode.Simple,
                TermType = TermType.Monthly,
                TermCount = 12,
                LiquidationRate = 0m,
                TotalAmount = 10500m,
                RemainingBalance = 8750m,
                AmountPerTerm = 875m,
                StartDate = new DateOnly(2026, 1, 15),
                DueDate = new DateOnly(2027, 1, 15),
                Status = ContractStatus.Active,
                Notes = "",
                CreatedAt = now,
                UpdatedAt = now
            },
            new Contract
            {
                Id = 2,
                BorrowerId = 2,
                PrincipalAmount = 20000m,
                InterestRate = 3m,
                InterestMode = InterestMode.Compound,
                TermType = TermType.Monthly,
                TermCount = 6,
                LiquidationRate = 0m,
                TotalAmount = 20600m,
                RemainingBalance = 20600m,
                AmountPerTerm = 3433.33m,
                StartDate = new DateOnly(2026, 2, 1),
                DueDate = new DateOnly(2026, 8, 1),
                Status = ContractStatus.Active,
                Notes = "",
                CreatedAt = now,
                UpdatedAt = now
            });

        modelBuilder.Entity<Payment>().HasData(
            new Payment
            {
                Id = 1,
                ContractId = 1,
                Amount = 875m,
                PaymentDate = new DateOnly(2026, 2, 15),
                ReceiptNumber = "RCP-001",
                Notes = "",
                CreatedAt = new DateTime(2026, 2, 15, 10, 0, 0, DateTimeKind.Utc)
            },
            new Payment
            {
                Id = 2,
                ContractId = 1,
                Amount = 875m,
                PaymentDate = new DateOnly(2026, 3, 15),
                ReceiptNumber = "RCP-002",
                Notes = "",
                CreatedAt = new DateTime(2026, 3, 15, 10, 0, 0, DateTimeKind.Utc)
            });
    }
}
