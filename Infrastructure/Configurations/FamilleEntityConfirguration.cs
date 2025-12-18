using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class FamilleEntityConfirguration : IEntityTypeConfiguration<FamilleEntity>
{
    public void Configure(EntityTypeBuilder<FamilleEntity> builder)
    {
        builder.ToTable("Familles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        
        builder.HasMany(x => x.Products)
            .WithOne(x => x.Famille)
            .HasForeignKey(x => x.FamilleId);
    }
}