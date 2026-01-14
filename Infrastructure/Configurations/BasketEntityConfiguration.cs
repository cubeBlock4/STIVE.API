using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class BasketEntityConfiguration : IEntityTypeConfiguration<BasketEntity>
{
    public void Configure(EntityTypeBuilder<BasketEntity> builder)
    {
        builder.ToTable("Baskets");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CustomerId).HasColumnName("CustomerId").IsRequired();
        builder.HasOne(x => x.Customer).WithOne(x => x.Basket).HasForeignKey<BasketEntity>(x => x.CustomerId);
        builder.HasMany(x => x.Items).WithOne(x => x.Basket).HasForeignKey(x => x.BasketId);
    }
}