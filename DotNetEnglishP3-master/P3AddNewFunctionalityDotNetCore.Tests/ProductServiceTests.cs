using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Tests.Stubs;

using Xunit;


namespace P3AddNewFunctionalityDotNetCore.Tests
{

    public class ProductServiceValidationTests
    {
        private readonly ProductService _productService;

        public ProductServiceValidationTests()
        {
            _productService = new ProductService(
                   new CartStub(), 
                   new ProductRepositoryStub(), 
                   new OrderRepositoryStub(),   
                   new StringLocalizerStub()   
            );
        }

        [Fact]
        public void CheckProduct_Should_ReturnMissingName_When_NameIsEmpty()
        {
            var product = new ProductViewModel { Name = "", Price = "10", Stock = "5" };

            var result = _productService.CheckProduct(product);

            Assert.Contains("Veuillez saisir un nom", result);
        }

        [Fact]
        public void CheckProduct_Should_ReturnMissingPrice_When_PriceIsEmpty()
        {
            var product = new ProductViewModel { Name = "Produit", Price = "", Stock = "5" };

            var result = _productService.CheckProduct(product);

            Assert.Contains("Veuillez saisir un prix", result);
        }

        [Fact]
        public void CheckProduct_Should_ReturnPriceNotGreaterThanZero_When_PriceIsZero()
        {
            var product = new ProductViewModel { Name = "Produit", Price = "0", Stock = "5" };

            var result = _productService.CheckProduct(product);

            Assert.Contains("Le prix doit être un nombre supérieur à zéro", result);
        }

        [Fact]
        public void CheckProduct_Should_ReturnPriceNotANumber_When_PriceIsNotDecimal()
        {
            var product = new ProductViewModel { Name = "Produit", Price = "abc", Stock = "5" };

            var result = _productService.CheckProduct(product);

            Assert.Contains("Le prix doit être un nombre supérieur à zéro", result[0]);
        }

        [Fact]
        public void CheckProduct_Should_ReturnMissingQuantity_When_StockIsEmpty()
        {
            var product = new ProductViewModel { Name = "Produit", Price = "10", Stock = "" };

            var result = _productService.CheckProduct(product);

            Assert.Contains("Veuillez saisir un stock", result);
        }

        [Fact]
        public void CheckProduct_Should_ReturnQuantityNotAnInteger_When_StockIsFloat()
        {
            var product = new ProductViewModel { Name = "Produit", Price = "10", Stock = "3.14" };

            var result = _productService.CheckProduct(product);
            
            Assert.Contains("Le stock doit être un nombre supérieur à zéro", result[0]);
        }

        [Fact]
        public void CheckProduct_Should_ReturnQuantityNotGreaterThanZero_When_StockIsZero()
        {
            var product = new ProductViewModel { Name = "Produit", Price = "10", Stock = "0" };

            var result = _productService.CheckProduct(product);

            Assert.Contains("Le stock doit être un nombre supérieur à zéro", result);
        }

        [Fact]
        public void CheckProduct_Should_ReturnNoError_When_ValidProduct()
        {
            var product = new ProductViewModel { Name = "Produit valide", Price = "10.99", Stock = "7" };

            var result = _productService.CheckProduct(product);

            Assert.Empty(result);
        }
    }
}
