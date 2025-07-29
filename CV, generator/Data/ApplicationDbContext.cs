using CV_generator.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json; // For JSON serialization/deserialization
using System.Collections.Generic; // Ensure this is present for List<string>

namespace CV_generator.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ResumeData> Resumes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure ResumeData entity
            modelBuilder.Entity<ResumeData>(entity =>
            {
                // Configure complex types to be owned by ResumeData
                entity.OwnsOne(r => r.Personal, personal =>
                {
                    // No explicit configuration needed for simple owned type properties
                });

                // Configure collections of complex types (Owned Entities)
                entity.OwnsMany(r => r.Education, education =>
                {
                    education.ToJson(); // Store Education as JSON array within ResumeData table
                });

                entity.OwnsMany(r => r.Experience, experience =>
                {
                    experience.ToJson(); // Store Experience as JSON array within ResumeData table
                    // For Experience.Description (List<string>), ensure conversion if needed for nested lists
                    // If Description is a List<string>, it will be serialized as part of the Experience JSON.
                    // No separate HasConversion needed here unless Description itself is a complex object.
                    // If you want Description to be a separate column, you'd configure it differently.
                    // For List<string> within an owned entity that is ToJson(), EF Core handles serialization.
                });

                entity.OwnsMany(r => r.Projects, project =>
                {
                    project.ToJson(); // Store Projects as JSON array within ResumeData table
                });

                // For Skills (List<string>), store as JSON string in a column within ResumeData table
                entity.Property(r => r.Skills)
                      .HasConversion(
                          v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
                          v => JsonSerializer.Deserialize<List<string>>(v, new JsonSerializerOptions())
                      );
            });
        }
    }
}
