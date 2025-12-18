using Core.Dto;
using Infrastructure.Configurations;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class StiveContext : DbContext
{
    public StiveContext(DbContextOptions<StiveContext> options) : base(options)
    {
    }
    
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<SupplierEntity> Suppliers { get; set; }
    public DbSet<FamilleEntity> Familles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CustomerEntityConfirguration());
        modelBuilder.ApplyConfiguration(new SupplierEntityConfirguration());
        modelBuilder.ApplyConfiguration(new FamilleEntityConfirguration());
    }
}