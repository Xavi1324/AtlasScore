using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.DBContext;
using Persistence.Interfaces.IRepositories;
using Persistence.Repositories;

namespace Persistence
{
    public static class ServicesRegistrarion
    {
        public static void AddPersitence(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<AtlasScoreDbContext>(options => options.UseInMemoryDatabase("AtlasScoreDbMemory"));
            }
            else
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
               
                services.AddDbContext<AtlasScoreDbContext>(options =>
                options.UseSqlServer(connectionString, m => m.MigrationsAssembly(typeof(AtlasScoreDbContext).Assembly.FullName)));
            }

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));



        }
    }
}
