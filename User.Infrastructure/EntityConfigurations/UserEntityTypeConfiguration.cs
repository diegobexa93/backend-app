using BaseShare.Common.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using User.Domain.Entities;

namespace User.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<UserObj>
    {

        public override void Configure(EntityTypeBuilder<UserObj> builder)
        {
            // Additional configurations specific to UserObj
            builder.ToTable("User");

            base.Configure(builder); // Apply base configurations
            builder.Property(b => b.Password).HasMaxLength(300);

        }
    }
}
