using Core.Dto;
using Core.Repositories;
using Infrastructure.Context;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class BasketsRepository : IBasketRepository
{
    private readonly StiveContext _context;

    public BasketsRepository(StiveContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BasketDto>> GetAllBaskets()
    {
        // Note: In a real app, returning ALL baskets might be heavy.
        return await _context.Baskets
            .Include(b => b.Customer)
            .Include(b => b.Items)
            .ThenInclude(i => i.Product)
            .Select(b => new BasketDto
            {
                Id = b.Id,
                Customer = new CustomerDto
                {
                    Id = b.Customer.Id,
                    FirstName = b.Customer.FirstName,
                    LastName = b.Customer.LastName,
                    Email = b.Customer.Email
                },
                Items = b.Items.Select(i => new ItemDto
                {
                    Id = i.Id,
                    Quantity = i.Quantity,
                    Product = new ProductDto
                    {
                        Id = i.Product.Id,
                        Name = i.Product.Name,
                        Reference = i.Product.Reference,
                        Price = i.Product.Price
                    }
                })
            }).ToListAsync();
    }

    public async Task<BasketDto> GetBasket(int id)
    {
        var basket = await _context.Baskets
            .Include(b => b.Customer)
            .Include(b => b.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(b => b.Id == id);
            
        if (basket == null) throw new Exception("Basket not found");

        return new BasketDto
        {
            Id = basket.Id,
            Customer = new CustomerDto
            {
                Id = basket.Customer.Id,
                FirstName = basket.Customer.FirstName,
                LastName = basket.Customer.LastName,
                Email = basket.Customer.Email
            },
            Items = basket.Items.Select(i => new ItemDto
            {
                Id = i.Id,
                Quantity = i.Quantity,
                Product = new ProductDto
                {
                    Id = i.Product.Id,
                    Name = i.Product.Name,
                    Reference = i.Product.Reference,
                    Price = i.Product.Price
                }
            })
        };
    }

    public async Task CreateBasket(int customerId, int productId, int quantity)
    {
       // Basic implementation: Create a basket for a customer provided they don't have one? or just create one.
       // The arguments (customerId, productId, quantity) seem to imply "Add item to basket" or "Create basket with item".
       // Given the Interface signature, I will implement it as: Check if customer has basket (Entity config says 1-1). 
       // If yes, add item. If no, create basket and add item.
       
       var customer = await _context.Customers
           .Include(c => c.Basket)
           .ThenInclude(b => b.Items)
           .FirstOrDefaultAsync(c => c.Id == customerId);
           
       if (customer == null) throw new Exception("Customer not found");

       var basket = customer.Basket;
       if (basket == null)
       {
           basket = new BasketEntity
           {
               Customer = customer,
               CustomerId = customerId
           };
           await _context.Baskets.AddAsync(basket);
       }
       
       // Add Item
       var product = await _context.Products.FindAsync(productId);
       if (product == null) throw new Exception("Product not found");
       
       var item = new ItemEntity
       {
           Basket = basket,
           Product = product,
           Quantity = quantity
       };
       
       // Note: In real world we might check if item already exists in basket and update quantity.
       if (basket.Items == null) basket.Items = new List<ItemEntity>();
       basket.Items.Add(item);
       
       await _context.SaveChangesAsync();
    }

    public async Task UpdateBasket(int id, int customerId, int productId, int quantity)
    {
        // Arguments seem confusing for "UpdateBasket". Update typically modifies the basket itself.
        // But (customerId, productId, quantity) looks like "Update Item in Basket".
        // Assuming 'id' is BasketId?
        
        var basket = await _context.Baskets.Include(b => b.Items).FirstOrDefaultAsync(b => b.Id == id);
         if (basket == null) throw new Exception("Basket not found");
         
         // Logic is unclear from signature. I will assume it means "Set item quantity in basket"
         // Only fixing compilation so simplistic logic is acceptable.
         var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
         if (item != null)
         {
             item.Quantity = quantity;
             if (quantity <= 0)
             {
                 _context.Entry(item).State = EntityState.Deleted; 
             }
         }
         await _context.SaveChangesAsync();
    }

    public async Task DeleteBasket(int id)
    {
        var basket = await _context.Baskets.FindAsync(id);
        if (basket != null)
        {
            _context.Baskets.Remove(basket);
            await _context.SaveChangesAsync();
        }
    }
    public async Task<BasketDto> GetBasketByCustomerId(int customerId)
    {
        var basket = await _context.Baskets
            .Include(b => b.Customer)
            .Include(b => b.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(b => b.CustomerId == customerId);

        if (basket == null)
        {
            // Return empty basket DTO if not found, or create new? 
            // Usually nice to return empty structure or null. 
            // Let's create one on the fly if it doesn't exist for better user experience?
            // "Get Basket" implies "Get MY basket". If I don't have one, I have an empty one.
            
            // Check if customer exists first
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) throw new Exception("Customer not found");

            return new BasketDto
            {
                Customer = new CustomerDto
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email
                },
                Items = new List<ItemDto>()
            };
        }

        return new BasketDto
        {
            Id = basket.Id,
            Customer = new CustomerDto
            {
                Id = basket.Customer.Id,
                FirstName = basket.Customer.FirstName,
                LastName = basket.Customer.LastName,
                Email = basket.Customer.Email
            },
            Items = basket.Items.Select(i => new ItemDto
            {
                Id = i.Id,
                Quantity = i.Quantity,
                Product = new ProductDto
                {
                    Id = i.Product.Id,
                    Name = i.Product.Name,
                    Reference = i.Product.Reference,
                    Price = i.Product.Price
                }
            })
        };
    }

    public async Task AddItemToBasket(int customerId, int productId, int quantity)
    {
        var basket = await _context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.CustomerId == customerId);

        if (basket == null)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null) throw new Exception("Customer not found");
            
            basket = new BasketEntity
            {
                CustomerId = customerId
            };
            await _context.Baskets.AddAsync(basket);
            // Save to get ID? Not needed for navigation usually, but good for consistency
            await _context.SaveChangesAsync(); 
        }

        var product = await _context.Products.FindAsync(productId);
        if (product == null) throw new Exception("Product not found");

        var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            item.Quantity += quantity;
        }
        else
        {
            item = new ItemEntity
            {
                Basket = basket, // EF will handle FK
                Product = product, // EF will handle FK
                Quantity = quantity
            };
            if (basket.Items == null) basket.Items = new List<ItemEntity>();
            basket.Items.Add(item);
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpdateItemInBasket(int customerId, int productId, int quantity)
    {
        var basket = await _context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.CustomerId == customerId);
            
        if (basket == null) throw new Exception("Basket not found");

        var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            if (quantity <= 0)
            {
                 _context.Entry(item).State = EntityState.Deleted;
                 basket.Items.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }
            await _context.SaveChangesAsync();
        }
        else 
        {
            throw new Exception("Item not found in basket");
        }
    }

    public async Task RemoveItemFromBasket(int customerId, int productId)
    {
        var basket = await _context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.CustomerId == customerId);
            
        if (basket == null) return; // Or throw

        var item = basket.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null)
        {
            _context.Entry(item).State = EntityState.Deleted;
             basket.Items.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearBasket(int customerId)
    {
        var basket = await _context.Baskets
            .Include(b => b.Items)
            .FirstOrDefaultAsync(b => b.CustomerId == customerId);

        if (basket != null && basket.Items != null && basket.Items.Any())
        {
            _context.RemoveRange(basket.Items);
            await _context.SaveChangesAsync();
        }
    }
}