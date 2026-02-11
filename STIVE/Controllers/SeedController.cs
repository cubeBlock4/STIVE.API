using System.Security.Cryptography;
using System.Text;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace STIVE.Controllers;

[ApiController]
[Route("seed")]
public class SeedController : ControllerBase
{
    private readonly StiveContext _context;

    public SeedController(StiveContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Seed()
    {
        if (await _context.Customers.AnyAsync())
        {
            return BadRequest("Database seems to be already seeded (Customers exist).");
        }

        // 1. Familles
        var familles = new List<FamilleEntity>
        {
            new FamilleEntity { Name = "Fruits" },
            new FamilleEntity { Name = "Légumes" },
            new FamilleEntity { Name = "Boissons" },
            new FamilleEntity { Name = "Épicerie" }
        };
        await _context.Familles.AddRangeAsync(familles);
        await _context.SaveChangesAsync();

        // 2. Suppliers
        var suppliers = new List<SupplierEntity>
        {
            new SupplierEntity { EntrepriseName = "BioFarm", Email = "contact@biofarm.com", Phone = "0102030405" },
            new SupplierEntity { EntrepriseName = "LocalProd", Email = "info@localprod.fr", Phone = "0607080910" },
            new SupplierEntity { EntrepriseName = "GlobalDistrib", Email = "sales@global.com", Phone = "0199887766" }
        };
        await _context.Suppliers.AddRangeAsync(suppliers);
        await _context.SaveChangesAsync();

        // 3. Products
        var products = new List<ProductEntity>
        {
            new ProductEntity 
            { 
                Name = "Pomme Golden", 
                Reference = "FR-POM-001", 
                Price = "2.50", 
                Famille = familles[0], 
                Supplier = suppliers[0] 
            },
            new ProductEntity 
            { 
                Name = "Banane", 
                Reference = "INT-BAN-002", 
                Price = "1.80", 
                Famille = familles[0], 
                Supplier = suppliers[2] 
            },
            new ProductEntity 
            { 
                Name = "Carotte", 
                Reference = "FR-CAR-003", 
                Price = "1.20", 
                Famille = familles[1], 
                Supplier = suppliers[1] 
            },
            new ProductEntity 
            { 
                Name = "Jus d'Orange", 
                Reference = "ESP-JUS-004", 
                Price = "3.50", 
                Famille = familles[2], 
                Supplier = suppliers[2] 
            },
            new ProductEntity 
            { 
                Name = "Pâtes Fusilli", 
                Reference = "IT-PAT-005", 
                Price = "0.90", 
                Famille = familles[3], 
                Supplier = suppliers[2] 
            }
        };
        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();

        // 4. Customers
        // User 1: John Doe
        using var hmac1 = new HMACSHA512();
        var customer1 = new CustomerEntity
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@test.com",
            Role = "Customer",
            PasswordHash = Convert.ToBase64String(hmac1.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"))),
            PasswordSalt = hmac1.Key
        };
        
        // User 2: Jane Smith
        using var hmac2 = new HMACSHA512();
        var customer2 = new CustomerEntity
        {
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@test.com",
            Role = "Customer",
            PasswordHash = Convert.ToBase64String(hmac2.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"))),
            PasswordSalt = hmac2.Key
        };

        // Admin User
        using var hmacAdmin = new HMACSHA512();
        var admin = new CustomerEntity
        {
            FirstName = "Admin",
            LastName = "System",
            Email = "admin@stive.com",
            Role = "Admin",
            PasswordHash = Convert.ToBase64String(hmacAdmin.ComputeHash(Encoding.UTF8.GetBytes("Admin123!"))),
            PasswordSalt = hmacAdmin.Key
        };

        await _context.Customers.AddRangeAsync(customer1, customer2, admin);
        await _context.SaveChangesAsync();

        // 5. Baskets & Items
        // Basket for John
        var basket1 = new BasketEntity { Customer = customer1 };
        await _context.Baskets.AddAsync(basket1);
        await _context.SaveChangesAsync();

        var items1 = new List<ItemEntity>
        {
            new ItemEntity { Basket = basket1, Product = products[0], Quantity = 6 }, // 6 Pommes
            new ItemEntity { Basket = basket1, Product = products[3], Quantity = 2 }  // 2 Jus
        };
        
        // Basket for Jane
        var basket2 = new BasketEntity { Customer = customer2 };
        await _context.Baskets.AddAsync(basket2);
        await _context.SaveChangesAsync();

        var items2 = new List<ItemEntity>
        {
            new ItemEntity { Basket = basket2, Product = products[2], Quantity = 1 }, // 1 Carotte
            new ItemEntity { Basket = basket2, Product = products[4], Quantity = 3 }  // 3 Pâtes
        };

        // Using Set<ItemEntity> because there is no DbSet<ItemEntity> exposed directly if I remember correctly, 
        // or just to be safe.
        await _context.Set<ItemEntity>().AddRangeAsync(items1);
        await _context.Set<ItemEntity>().AddRangeAsync(items2);
        
        await _context.SaveChangesAsync();

        return Ok(new 
        { 
            Message = "Database seeded successfully", 
            Counts = new 
            { 
                Familles = familles.Count, 
                Suppliers = suppliers.Count, 
                Products = products.Count, 
                Customers = 3,
                Baskets = 2,
                Items = items1.Count + items2.Count
            } 
        });
    }
}
