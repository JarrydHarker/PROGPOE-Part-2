using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Part2Library.Models;
using PROGPOE_Part_2.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Azure.Core;

namespace PROGPOE_Part_2.Controllers
{
    public class AccountController : Controller
    {
        AgrEnergyDbContext context = AgrEnergyDbContext.GetContext();

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await AuthenticateUser(email, password);
                if (currentUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View();
                }

                await currentUser.loadUserDetails(context);

                var claims = new List<Claim>
                 {
                    new Claim(ClaimTypes.Name, currentUser.userName),
                    new Claim(ClaimTypes.Email, currentUser.email),
                    new Claim(ClaimTypes.Role, await currentUser.checkRole(context)),
                  };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // Persistent across sessions
                };

                // Sign in the user asynchronously
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Store the user's data in the session
                await storeUser(currentUser);

                // Redirect based on the user's role
                var userRoleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (userRoleClaim == "Employee")
                {
                    return RedirectToAction("Index", "Employee");
                }
                else if (userRoleClaim == "Farmer")
                {
                    return RedirectToAction("Index", "Farmer");
                }
                else
                {
                    // Handle other roles or default case
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout(bool isOut)
        {
            if (isOut)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();
                HttpContext.Response.Cookies.Delete("User");
            }
            return RedirectToAction("Login", "Account");
        }

        private Task<IUser> AuthenticateUser(string email, string password)
        {
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                IUser user = new Farmer("", "", email, password, "");

                return user.authenticateUser(context, email, password);
            }
            else return null;

        }

        private async Task storeUser(IUser currentUser)
        {
            var userData = new
            {
                User = currentUser,
                Role = currentUser.checkRole(AgrEnergyDbContext.GetContext())
            };

            // Serialize the user data along with the role into JSON
            var userDataJson = JsonConvert.SerializeObject(userData);

            // Store the serialized user data in the session
            HttpContext.Session.SetString("CurrentUser", userDataJson);
        }
    }
}
