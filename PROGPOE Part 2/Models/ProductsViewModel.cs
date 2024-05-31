using Microsoft.AspNetCore.Mvc.Rendering;

namespace PROGPOE_Part_2.Models
{
    
    public class ProductsViewModel
    {
        public List<Product> lstProducts { get; set; }
        public Dictionary<string, string> productImages { get; set; }
        public IEnumerable<SelectListItem> lstCategories { get; set; }
        public Product newProduct { get; set; }

        public ProductsViewModel() { }
    }
}
