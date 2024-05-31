using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Part2Library.Models;
using PROGPOE_Part_2.Models;

namespace PROGPOE_Part_2.Controllers
{
    public class ProductsController : Controller
    {
        AgrEnergyDbContext context = AgrEnergyDbContext.GetContext();
        ImageAPI imageAPI = new ImageAPI();

        // GET: ProductsController
        public async Task<IActionResult> Index(string nameFilter, string categoryFilter, DateOnly? startDateFilter, DateOnly? endDateFilter)
        {
            List<Product> lstProducts = getProducts().ToList();

            if (!string.IsNullOrEmpty(nameFilter))
            {
                lstProducts = lstProducts.Where(p => p.productName.ToLower().Contains(nameFilter.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(categoryFilter))
            {
                lstProducts = lstProducts.Where(p => p.category == categoryFilter).ToList();
                Console.WriteLine(categoryFilter);
            }

            if (startDateFilter != null)
            {
                lstProducts = lstProducts.Where(p => p.productionDate >= startDateFilter.Value).Select(x => x).ToList();
            }

            if (endDateFilter != null)
            {
                lstProducts = lstProducts.Where(p => p.productionDate <= endDateFilter.Value).Select(x => x).ToList();
            }

            if (lstProducts != null)
            {
                var ImageUrls = new Dictionary<string, string>();

                foreach (var product in lstProducts)
                {
                    var imageUrl = await imageAPI.makeGetRequest("Products", product.productID);
                    ImageUrls.Add(product.productID,imageUrl);
                }

                ProductsViewModel model = new ProductsViewModel
                {
                    lstProducts = lstProducts.ToList(),
                    productImages = ImageUrls,
                    lstCategories = getCategories()
            };


                return View(model);
            }
            else return View();

        }

        // GET: ProductsController/Details/5
        public ActionResult Details(string id)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "FarmerOnly")]
        public IActionResult Edit(string id)
        {
            var product = context.TblProducts.Where(x => x.ProductId == id).FirstOrDefault();

            if (product != null)
            {
                return View(product);
            }else return RedirectToAction("Index", "Farmer");
            
        }

        // GET: ProductsController/Create
        [Authorize(Policy = "FarmerOnly")]
        public ActionResult Create()
        {
            ProductsViewModel model = new ProductsViewModel
            {
                lstCategories = getCategories()
            };
            return View(model);
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "FarmerOnly")]
        public ActionResult Create(Product product)
        {
            try
            {
                int numProducts = context.TblProducts.Count() + 1;
                string productID = "P00" + numProducts;
                Farmer? farmer = IUser.getCurrentUser(HttpContext, context) as Farmer;
                string? categoryID = context.TblCategories.Where(x => x.Name == product.category).Select(x => x.CategoryId).FirstOrDefault().ToString();

                if (farmer != null && categoryID != null)
                {
                    context.TblProducts.Add(new TblProduct()
                    {
                        ProductId = productID,
                        UserId = farmer.userID,
                        Name = product.productName,
                        Description = product.description,
                        Price = product.price,
                        CategoryId = categoryID,
                        ProductionDate = product.productionDate
                    });
                }
                else throw new Exception("Unexpected error occurred");

                context.SaveChanges();

                return RedirectToAction("Index", "Farmer");
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: ProductsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "FarmerOnly")]
        public ActionResult Delete(string id)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private IEnumerable<SelectListItem> getCategories()
        {
            // Fetch categories from the database and map to SelectListItem
            return context.TblCategories.Select(c => new SelectListItem
            {
                Value = c.Name,
                Text = c.Name
            }).ToList();
        }

        private IEnumerable<Product> getProducts()
        {
            // Fetch categories from the database and map to SelectListItem
            List<TblProduct> products = context.TblProducts.Select(c => c).Include(p => p.Category).ToList();


            return Product.mapToProducts(products);
        }

    }
}
