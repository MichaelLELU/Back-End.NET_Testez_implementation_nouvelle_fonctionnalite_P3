using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;
using System;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        public IServiceProvider Services { get; private set; } = null!;

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Authentification fictive
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                services.PostConfigureAll<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });

                // Supprimer le DbContext existant
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<P3Referential>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<P3Referential>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // Sauvegarde du service provider pour accès ultérieur
                Services = services.BuildServiceProvider();

                // Initialisation de la base
                using var scope = Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<P3Referential>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                SeedData(db);
            });

            return base.CreateHost(builder);
        }

        public void ResetDatabase()
        {
            using var scope = Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<P3Referential>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            SeedData(db);
        }

        private void SeedData(P3Referential db)
        {
            db.Product.AddRange(
                new Models.Entities.Product { Id = 1, Name = "Dummy", Price = 99.99, Quantity = 10, Description = "Desc", Details = "Details" },
                new Models.Entities.Product { Id = 2, Name = "Testos", Price = 100, Quantity = 5, Description = "Desc Test", Details = "mouais" },
                new Models.Entities.Product { Id = 3, Name = "Test Product 2", Price = 50.00, Quantity = 20, Description = "Desc 2", Details = "Details 2" }
            );
            db.SaveChanges();
        }
    }
}