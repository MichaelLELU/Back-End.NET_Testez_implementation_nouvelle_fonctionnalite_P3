using System;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace P3AddNewFunctionalityDotNetCore.Resources.Models
{
    public static class Product
    {
        private static ResourceManager resourceManager = new ResourceManager("P3AddNewFunctionalityDotNetCore.Resources.Models.Product", Assembly.GetExecutingAssembly());

        private static CultureInfo resourceCulture;

        public static string ErrorMissingName => resourceManager.GetString("ErrorMissingName", resourceCulture);
        public static string ErrorMissingPrice => resourceManager.GetString("ErrorMissingPrice", resourceCulture);
        public static string ErrorPriceValue => resourceManager.GetString("ErrorPriceValue", resourceCulture);
        public static string ErrorMissingStock => resourceManager.GetString("ErrorMissingStock", resourceCulture);
        public static string ErrorStockValue => resourceManager.GetString("ErrorStockValue", resourceCulture);
    }
}
