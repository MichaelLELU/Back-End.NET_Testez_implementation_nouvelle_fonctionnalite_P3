using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {

                // Mock l'authentification
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                services.PostConfigureAll<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                });

                // Supprimer l'ancien DbContext P3Referential
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<P3Referential>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Ajouter un DbContext InMemory pour les tests
                services.AddDbContext<P3Referential>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                // (optionnel) Ajouter des données seed pour les tests ici si tu veux
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<P3Referential>();
                db.Database.EnsureCreated();

                // Exemple de produit seed (optionnel)
                db.Product.AddRange(
                    new Models.Entities.Product
                { Id = 1, Name = "Marche", Price = 99.99, Quantity = 10, Description = "Desc", Details = "Details" },
                    new Models.Entities.Product
                { Id = 2, Name = "Testos", Price = 100, Quantity = 5, Description = "Desc Test", Details = "mouais"},
                    new Models.Entities.Product
                { Id = 3, Name = "Test Product 2", Price = 50.00, Quantity = 20, Description = "Desc 2", Details = "Details 2" });
                db.SaveChanges();
            });

            return base.CreateHost(builder);
        }
    }
}
