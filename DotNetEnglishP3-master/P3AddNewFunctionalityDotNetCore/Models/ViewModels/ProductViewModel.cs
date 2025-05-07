using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;



namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Models.Product), 
           ErrorMessageResourceName = "ErrorMissingName")]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Models.Product),
            ErrorMessageResourceName = "ErrorMissingPrice")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$",
            ErrorMessageResourceType = typeof(Resources.Models.Product),
            ErrorMessageResourceName = "ErrorPriceValue")]
        [Range(0.01, double.MaxValue,
            ErrorMessageResourceType = typeof(Resources.Models.Product),
            ErrorMessageResourceName = "ErrorPriceValue")]
        public string Price { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(Resources.Models.Product),
            ErrorMessageResourceName = "ErrorMissingStock")]
        [RegularExpression(@"^\d+$",
            ErrorMessageResourceType = typeof(Resources.Models.Product),
            ErrorMessageResourceName = "ErrorStockValue")]
        [Range(1, int.MaxValue,
            ErrorMessageResourceType = typeof(Resources.Models.Product),
            ErrorMessageResourceName = "ErrorStockValue")]
        public string Stock { get; set; }
    }
}
