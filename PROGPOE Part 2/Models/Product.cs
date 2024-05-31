using Part2Library.Models;
using System.ComponentModel.DataAnnotations;

namespace PROGPOE_Part_2.Models
{
    
    public class Product
    {
        public string? productID { get; set; }
        public string productName { get; set; }
        public string? description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = true)]
        public decimal price { get; set; }

        public string? category { get; set; }
        public DateOnly productionDate { get; set; }

        public Product(string productID, string productName, string? description, decimal price, string? category, DateOnly productionDate)
        {
            this.productID = productID;
            this.productName = productName;
            this.description = description;
            this.price = price;
            this.category = category;
            this.productionDate = productionDate;
        }

        public Product() { }

        public Product(TblProduct product) 
        {
            productID = product.ProductId;
            productName = product.Name;
            description = product.Description;
            price = product.Price;
            productionDate = product.ProductionDate;

            if (product.Category != null)
            {
                category = product.Category.Name;
            }
            
            
        }

        public static List<Product> mapToProducts(List<TblProduct> tblProducts)
        {
            return tblProducts.Select(tblProduct => new Product(
            tblProduct.ProductId,
            tblProduct.Name,
            tblProduct.Description,
            tblProduct.Price,
            tblProduct.Category?.Name,
            tblProduct.ProductionDate
            )).ToList();
        }

        public static List<TblCategory> getCategories(AgrEnergyDbContext context)
        {
            List<TblCategory> categories = context.TblCategories.Select(x => x).ToList();

            if (categories != null)
            {
                return categories;
            }else return categories = new List<TblCategory>();
        }
    }
}
