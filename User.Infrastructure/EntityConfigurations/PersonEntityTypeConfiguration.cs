using BaseShare.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Entities;

namespace User.Infrastructure.EntityConfigurations
{
    public class PersonEntityTypeConfiguration : BaseEntityTypeConfiguration<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> personConfiguration)
        {
            personConfiguration.ToTable("Person");
            base.Configure(personConfiguration); // Apply base configurations


            personConfiguration.Property(b => b.Name).HasMaxLength(100);
            personConfiguration.Property(b => b.Email).HasMaxLength(100);

            // Create a unique index on the CPF property
            personConfiguration.HasIndex(p => p.Email)
                .IsUnique()
                .HasDatabaseName("IX_Person_Email");
        }
    }
}
