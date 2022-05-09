using Identity.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure.Persistence
{
    public class UserSeed
    {
        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    await EnsureSeedData(context);
                }
            }
        }

        private static async Task EnsureSeedData(ApplicationDbContext context)
        {
            Console.WriteLine("Seeding database...");

            if (!context.Users.Any())
            {
                Console.WriteLine("Users being populated");
                var user = new User
                {
                    Username = "salman1277",
                    Password = "password"
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Users already populated");
            }


            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }
    }
}
