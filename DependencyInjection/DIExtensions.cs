using Core.Repositories;
using Infrastructure;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjection;

public static class DIExtensions
{
    public static void AddRepositories(this IServiceCollection services, IConfiguration  configuration)
    {
        var connectionString = configuration.GetConnectionString("StiveDb");
        var databaseOptions = new DatabaseOptions()
        {
            ConnectionString = connectionString
        };
        services.AddSingleton(databaseOptions);
        services.AddDbContext<StiveContext>(
            options => options.UseSqlServer(connectionString)
        );

        services.AddTransient<ICustomerRepository, CustomerRepository>();
    }
}