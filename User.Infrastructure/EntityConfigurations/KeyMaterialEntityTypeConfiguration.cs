using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDevPack.Security.Jwt.Core.Model;

namespace User.Infrastructure.EntityConfigurations
{
    public class KeyMaterialEntityTypeConfiguration : IEntityTypeConfiguration<KeyMaterial>
    {
        public void Configure(EntityTypeBuilder<KeyMaterial> configuration)
        {
            configuration.ToTable("KeyMaterials");
            configuration.HasKey(k => k.Id);

        }
    }
}
