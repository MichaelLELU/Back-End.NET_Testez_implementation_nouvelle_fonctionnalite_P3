using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P3AddNewFunctionalityDotNetCore.Tests.Stubs
{
    internal class OrderRepositoryStub : IOrderRepository
    {
        public Task<Order> GetOrder(int? id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IList<Order>> GetOrders()
        {
            throw new System.NotImplementedException();
        }

        public void Save(Order order)
        {
            throw new System.NotImplementedException();
        }
    }
}