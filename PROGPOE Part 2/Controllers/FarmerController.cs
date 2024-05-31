using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Part2Library.Models;
using PROGPOE_Part_2.Models;

namespace PROGPOE_Part_2.Controllers
{
    public class FarmerController : Controller
    {
        AgrEnergyDbContext context = AgrEnergyDbContext.GetContext();
        ImageAPI imageAPI = new ImageAPI();

        [Authorize(Policy = "FarmerOnly")]
        public async Task<IActionResult> Index()
        {
            IUser currentUser = IUser.getCurrentUser(HttpContext, context);

            if (currentUser != null && currentUser is Farmer)
            {
                string imageUrl = await imageAPI.makeGetRequest("Users", currentUser.userID);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    ViewData["ProfilePic"] = imageUrl;
                }
                else ViewData["ProfilePic"] = "~/Images/DefaultProfile.png";

                Farmer user = (Farmer)currentUser;
                ViewData["CurrentUser"] = user;

                return View(user);
            }
            else
            {
                // Redirect to login page
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        [Authorize(Policy = "EmployeeOnly")]
        public IActionResult Delete(string id)
        {
            try
            {
                context.TblUsers.Where(x => x.UserId == id).ExecuteDelete();
                context.SaveChanges();

                return RedirectToAction("Index", "Employee");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;

                return RedirectToAction("Index", "Employee");
            }
            
        }

        [HttpGet]
        [Authorize(Policy = "EmployeeOnly")]
        public IActionResult Create()
        {
              return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EmployeeOnly")]
        public IActionResult Create(Farmer farmer)
        {
            try
            {
                context.TblUsers.Add(new TblUser
                {
                    UserId = "U00" +(context.TblUsers.Count() + 1).ToString(),
                    Username = farmer.userName,
                    Email = farmer.email,
                    Password = farmer.password,
                    RoleId = 2.ToString()
                });

                context.SaveChanges();

                return RedirectToAction("Index", "Employee");

            }catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Create");
            }
            

            
        }

    }
}
