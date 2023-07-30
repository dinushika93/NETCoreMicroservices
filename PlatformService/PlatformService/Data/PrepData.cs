using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Data
{
    public static class PrepData
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
            }
        }
        private static void SeedData(AppDbContext context, bool isProduction)
        {
            if (isProduction)
            {
                Console.WriteLine("Applying migrations..");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in adding migrations: {ex.Message}");
                }
                
            }
                if (!context.Platforms.Any())
                {
                    Console.WriteLine("Seeding data...");
                    context.Platforms.AddRange(
                     new Platform() { Name = "Dot Net", Publisher = "Microsoft", Cost = "Free" },
                     new Platform() { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                     new Platform() { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                    );
                    context.SaveChanges();
                }
        }
    }
}
