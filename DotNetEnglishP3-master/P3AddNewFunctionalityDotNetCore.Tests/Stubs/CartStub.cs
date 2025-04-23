using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;

namespace P3AddNewFunctionalityDotNetCore.Tests.Stubs
{
    internal class CartStub : ICart
    {
        public void AddItem(Product product, int quantity)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public double GetAverageValue()
        {
            throw new System.NotImplementedException();
        }

        public double GetTotalValue()
        {
            throw new System.NotImplementedException();
        }

        public void RemoveLine(Product product)
        {
            throw new System.NotImplementedException();
        }
    }
}