using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class SupplierEntityConfirguration : IEntityTypeConfiguration<SupplierEntity>
{
    public void Configure(EntityTypeBuilder<SupplierEntity> builder)
    {
        builder.ToTable("Suppliers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.EntrepriseName).HasColumnName("EntrepriseName").IsRequired();
        builder.Property(x => x.Email).HasColumnName("Email").IsRequired();
        builder.Property(x => x.Phone).HasColumnName("Phone").IsRequired();
        
        builder.HasMany(x => x.Products)
            .WithOne(x => x.Supplier)
            .HasForeignKey(x => x.SupplierId);
    }
}