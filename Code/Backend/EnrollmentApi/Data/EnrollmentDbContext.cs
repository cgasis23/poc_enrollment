using Microsoft.EntityFrameworkCore;
using EnrollmentApi.Models;

namespace EnrollmentApi.Data
{
    public class EnrollmentDbContext : DbContext
    {
        public EnrollmentDbContext(DbContextOptions<EnrollmentDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<EnrollmentDocument> EnrollmentDocuments { get; set; }
        public DbSet<EnrollmentNote> EnrollmentNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customer configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Ssn);
                
                // Configure relationships
                entity.HasMany(e => e.Documents)
                      .WithOne(e => e.Customer)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                entity.HasMany(e => e.Notes)
                      .WithOne(e => e.Customer)
                      .HasForeignKey(e => e.CustomerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // EnrollmentDocument configuration
            modelBuilder.Entity<EnrollmentDocument>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FilePath).IsRequired();
                entity.Property(e => e.FileSize).IsRequired();
            });

            // EnrollmentNote configuration
            modelBuilder.Entity<EnrollmentNote>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();
            });

            // Seed some sample data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Sample customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "555-123-4567",
                    Address = "123 Main St",
                    City = "Anytown",
                    State = "CA",
                    ZipCode = "90210",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Status = EnrollmentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                },
                new Customer
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumber = "555-987-6543",
                    Address = "456 Oak Ave",
                    City = "Somewhere",
                    State = "NY",
                    ZipCode = "10001",
                    DateOfBirth = new DateTime(1985, 5, 15),
                    Status = EnrollmentStatus.InProgress,
                    CreatedAt = DateTime.UtcNow
                }
            );

            // Sample notes
            modelBuilder.Entity<EnrollmentNote>().HasData(
                new EnrollmentNote
                {
                    Id = 1,
                    CustomerId = 1,
                    Title = "Initial Contact",
                    Content = "Customer contacted us about enrollment process",
                    Author = "System",
                    Type = NoteType.General,
                    CreatedAt = DateTime.UtcNow
                },
                new EnrollmentNote
                {
                    Id = 2,
                    CustomerId = 2,
                    Title = "Document Upload",
                    Content = "Customer uploaded required documents",
                    Author = "System",
                    Type = NoteType.Document,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
