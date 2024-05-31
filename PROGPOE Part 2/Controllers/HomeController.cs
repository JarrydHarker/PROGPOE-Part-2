using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using PROGPOE_Part_2.Models;
using System.Diagnostics;
using System.Security.Claims;
using Part2Library.Models;

namespace PROGPOE_Part_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewData["CurrentUser"] = IUser.getCurrentUser(HttpContext, AgrEnergyDbContext.GetContext());
            }
            
            ViewData["Categories"] = Product.getCategories(AgrEnergyDbContext.GetContext());
            ViewData["ProfilePic"] = "~/Images/DefaultProfile.png";

            ImageAPI imageAPI = new ImageAPI();

            var Categories = Product.getCategories(AgrEnergyDbContext.GetContext()) as List<TblCategory>;
            var ImageUrls = new Dictionary<string, string>();


            foreach (var category in Categories)
            {
                var imageUrl = await imageAPI.makeGetRequest("Categories", category.CategoryId);
                ImageUrls.Add(category.CategoryId, imageUrl);
            }


            HomeViewModel model = new HomeViewModel
            {
                ImageUrls = ImageUrls,
                lstCategories = Categories
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
