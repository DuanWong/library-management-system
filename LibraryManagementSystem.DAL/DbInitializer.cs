using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagementSystem.DAL
{
    public static class DbInitializer
    {
        public static void SeedDatabase(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

                if (!context.Subjects.Any())
                {
                    var sqlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqlSeedData", "SeedData.sql");

                    if (File.Exists(sqlPath))
                    {
                        var sql = File.ReadAllText(sqlPath);
                        context.Database.ExecuteSqlRaw(sql);
                    }
                }
            }
        }
    }
}
