using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using System.Collections.Generic;

namespace P3AddNewFunctionalityDotNetCore.Tests.Stubs
{
    internal class StringLocalizerStub : IStringLocalizer<ProductService>
    {
        public LocalizedString this[string name] => throw new System.NotImplementedException();

        public LocalizedString this[string name, params object[] arguments] => throw new System.NotImplementedException();

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new System.NotImplementedException();
        }
    }
}