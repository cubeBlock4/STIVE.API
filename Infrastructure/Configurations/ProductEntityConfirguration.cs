using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ProductEntityConfirguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
        builder.Property(x => x.Reference).HasColumnName("Reference").IsRequired();
        builder.Property(x => x.Price).HasColumnName("Price").IsRequired();
        
        builder.Property(x => x.FamilleId).HasColumnName("FamilleId");
        builder.HasOne(x => x.Famille)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.FamilleId);
        
        builder.Property(x => x.SupplierId).HasColumnName("SupplierId");
        builder.HasOne(x => x.Supplier)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.SupplierId);
    }
}