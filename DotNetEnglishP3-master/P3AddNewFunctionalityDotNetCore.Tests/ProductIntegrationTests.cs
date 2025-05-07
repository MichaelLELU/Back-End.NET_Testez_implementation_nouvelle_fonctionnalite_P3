using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {

        private readonly HttpClient _client;
        public ProductIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Index_ShouldReturnSuccessAndContainProduct()
        {
            // Act
            var response = await _client.GetAsync("/Product/Index");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            Assert.Contains("Test Product", content);
        }

        [Fact]
        public async Task Create_Post_ShouldCreateProduct_WhenAuthenticated()
        {
            var postData = new StringContent("Name=NewProduct&Price=10&Stock=5&Description=Test&Details=Details", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await _client.PostAsync("/Product/Create", postData);


            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Product/Admin", response.Headers.Location.ToString());

            // Vérifie que le produit apparaît
            var indexResponse = await _client.GetAsync("/Product/Admin");
            var indexContent = await indexResponse.Content.ReadAsStringAsync();
            Assert.Contains("NewProduct", indexContent);
        }

        [Fact]
        public async Task DeletedProduct_ShouldNotAppearInProductIndex()
        {
            var postData = new StringContent("id=1", Encoding.UTF8, "application/x-www-form-urlencoded");

            var response = await _client.PostAsync("/Product/DeleteProduct", postData);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Product/Admin", response.Headers.Location.ToString());


            var indexResponse = await _client.GetAsync("/Product/Admin");
            var content = await indexResponse.Content.ReadAsStringAsync();
            Assert.DoesNotContain("Marche", content);
        }

        [Fact]
        public async Task DeletedProduct_ShouldNotAppearInProductIndex_ForClient()
        {
            // Arrange : suppression du produit via l'interface admin
            var postData = new StringContent("id=2", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await _client.PostAsync("/Product/DeleteProduct", postData);

            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Product/Admin", response.Headers.Location.ToString());

            // Act : vérification que le produit ne s'affiche plus dans la vue publique
            var indexResponse = await _client.GetAsync("/Product/Index");
            var content = await indexResponse.Content.ReadAsStringAsync();

            // Assert : le produit supprimé ne doit pas apparaître côté client
            Assert.DoesNotContain("Testos", content);
        }

        [Fact]
        public async Task DeletedProduct_ShouldNotBeAddableToCart()
        {
            var deleteData = new StringContent("id=3", Encoding.UTF8, "application/x-www-form-urlencoded");
            var deleteResponse = await _client.PostAsync("/Product/DeleteProduct", deleteData);
            Assert.Equal(HttpStatusCode.Redirect, deleteResponse.StatusCode);


            var addToCartResponse = await _client.PostAsync("/Cart/AddProductToCart/3", null);
            var cartContent = await addToCartResponse.Content.ReadAsStringAsync();


            Assert.DoesNotContain("Test Product 2", cartContent);
            Assert.Contains("", cartContent, StringComparison.OrdinalIgnoreCase); 
        }
    }
}
