using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Part2Library.Models;
using PROGPOE_Part_2.Models;

namespace PROGPOE_Part_2.Controllers
{
    public class EmployeeController : Controller
    {
        AgrEnergyDbContext context = AgrEnergyDbContext.GetContext();
        ImageAPI imageAPI = new ImageAPI();

        [Authorize(Policy = "EmployeeOnly")]
        public async Task<IActionResult> Index()
        {
            IUser currentUser = IUser.getCurrentUser(HttpContext, context);

            if (currentUser != null && currentUser is Employee)
            {
                ViewData["CurrentUser"] = currentUser;
                ViewData["Role"] = "Employee";
                var lstFarmers = context.TblUsers.Where(x => x.RoleId == "2").ToList();
                Dictionary<string, string> farmerProfiles = new Dictionary<string, string>();
                foreach (var farmer in lstFarmers)
                {
                    string farmerUrl = await imageAPI.makeGetRequest("Users", farmer.UserId);
                    farmerProfiles.Add(farmer.UserId ,farmerUrl);
                }

                // Cast to Employee if the user is an Employee
                Employee employee = (Employee)currentUser;
                Console.WriteLine(currentUser.userName);

                string imageUrl = await imageAPI.makeGetRequest("Users", employee.userID);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    ViewData["ProfilePic"] = imageUrl;
                }
                else ViewData["ProfilePic"] = "~/Images/DefaultProfile.png";

                EmployeeViewModel model = new EmployeeViewModel
                {
                    lstFarmers = Farmer.mapToFarmer(lstFarmers),
                    farmerProfiles = farmerProfiles,
                    currentEmployee = employee
            };

                return View(model);
            }
            else
            {
                // Handle case where current user is null or not an Employee
                // Redirect to login page
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
