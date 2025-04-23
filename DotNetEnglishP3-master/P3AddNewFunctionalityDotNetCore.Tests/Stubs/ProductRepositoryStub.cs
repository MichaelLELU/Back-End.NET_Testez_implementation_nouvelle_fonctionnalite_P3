using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Tests.Stubs
{
    internal class ProductRepositoryStub : IProductRepository
    {
        public void DeleteProduct(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            throw new System.NotImplementedException();
        }

        public Task<Product> GetProduct(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<Product>> GetProduct()
        {
            throw new System.NotImplementedException();
        }

        public void SaveProduct(Product product)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateProductStocks(int productId, int quantityToRemove)
        {
            throw new System.NotImplementedException();
        }
    }
}