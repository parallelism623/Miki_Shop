using Ntier.DAL;
using Ntier.DAL.Context;

namespace Ntier.API
{
    public static class MigrationDatabase
    {
        public static async Task MigrateData(this WebApplication app)
        {
            using (var scoped = app.Services.CreateScope())
            {
                var context = scoped.ServiceProvider.GetService<ShopContext>();
                if (context != null)
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }
                await Seeder.SeedAsync(context);
            }
        }
    }
}
