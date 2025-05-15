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
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        public ProductIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Index_ShouldReturnSuccessAndContainProduct()
        {
            // Arrange
            _factory.ResetDatabase();

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
            // Arrange
            _factory.ResetDatabase();
            var postData = new StringContent(
                "Name=NewProduct&Price=10&Stock=5&Description=Test&Details=Details",
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            );

            // Act
            var response = await _client.PostAsync("/Product/Create", postData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Product/Admin", response.Headers.Location.ToString()); 

            // Act (accès à la liste admin)
            var indexResponse = await _client.GetAsync("/Product/Admin");
            var indexContent = await indexResponse.Content.ReadAsStringAsync();

            // Assert (le produit apparaît bien dans la liste)
            Assert.Contains("NewProduct", indexContent);
        }

        [Fact]
        public async Task DeletedProduct_ShouldNotAppearInProductIndex()
        {
            _factory.ResetDatabase();
            // Arrange
            var postData = new StringContent("id=1", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await _client.PostAsync("/Product/DeleteProduct", postData);

            // Act
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Product/Admin", response.Headers.Location.ToString());

            // Assert
            var indexResponse = await _client.GetAsync("/Product/Admin");
            var content = await indexResponse.Content.ReadAsStringAsync();
            Assert.DoesNotContain("Dummy", content);
        }

        [Fact]
        public async Task DeletedProduct_ShouldNotAppearInProductIndex_ForClient()
        {
            // Arrange
            _factory.ResetDatabase();
            var postData = new StringContent("id=1", Encoding.UTF8, "application/x-www-form-urlencoded");

            // Act
            var response = await _client.PostAsync("/Product/DeleteProduct", postData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Contains("/Product/Admin", response.Headers.Location.ToString());

            // Act (accès à la liste utilisateur)
            var indexResponse = await _client.GetAsync("/Product/Index");
            var content = await indexResponse.Content.ReadAsStringAsync();

            // Assert (le produit n'apparait pas dans la liste)
            Assert.DoesNotContain("Dummy", content);
        }

        [Fact]
        public async Task DeletedProduct_ShouldNotBeAddableToCart()
        {
            // Arrange
            _factory.ResetDatabase();

            // Act (ajouter au panier )
            var initialAddResponse = await _client.PostAsync("/Cart/AddToCart/1", null);

            // Assert (ajout réussi)
            Assert.Equal(HttpStatusCode.Redirect, initialAddResponse.StatusCode);

            // Act (vérifier contenu du panier)
            var cartResponse = await _client.GetAsync("/Cart");
            var initialCartContent = await cartResponse.Content.ReadAsStringAsync();

            // Assert (le produit est bien dans le panier)
            Assert.Contains("Dummy", initialCartContent);

            // Act (supprimer le produit)
            var deleteData = new StringContent("id=1", Encoding.UTF8, "application/x-www-form-urlencoded");
            var deleteResponse = await _client.PostAsync("/Product/DeleteProduct", deleteData);

            // Assert (suppression redirige correctement)
            Assert.Equal(HttpStatusCode.Redirect, deleteResponse.StatusCode);

            // Act (vérifier le panier après suppression)
            var cartAfterDeleteResponse = await _client.GetAsync("/Cart");
            var cartContentAfterDelete = await cartAfterDeleteResponse.Content.ReadAsStringAsync();

            // Assert (le produit n'est plus dans le panier)
            Assert.DoesNotContain("Dummy", cartContentAfterDelete);
        }

    }
}
