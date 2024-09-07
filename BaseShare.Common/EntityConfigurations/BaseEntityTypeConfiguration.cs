using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseShare.Common.EntityConfigurations
{
    public abstract class BaseEntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property("Id").UseMySqlIdentityColumn();
            builder.Property("GuidId").HasMaxLength(200).HasColumnType("varchar(200)");

            builder.Property("CreatedBy").HasMaxLength(200);
            builder.Property("UpdatedBy").HasMaxLength(200);

            // Unique index on GuidId
            builder.HasIndex("GuidId").IsUnique(true);
        }
    }
}
