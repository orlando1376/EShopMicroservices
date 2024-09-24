using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    /// <summary>
    /// Método de extensión para hacer automigraciones
    /// </summary>
    public static class Extentions
    {
        public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();
            dbContext.Database.MigrateAsync();

            return app;
        }
    }
}
