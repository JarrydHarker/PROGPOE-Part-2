using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Part2Library.Models;

namespace PROGPOE_Part_2.Models
{
    public interface IUser
    {
        string userID { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public void deleteUser(AgrEnergyDbContext context)
        {
            context.TblUsers.Where(x => x.UserId == userID).ExecuteDelete();
            context.SaveChanges();
        }

        public async Task<IUser> authenticateUser(AgrEnergyDbContext context, string email, string password)
        {
            // Fetch the user along with their role from the database
            var user = await context.TblUsers
                .Include(u => u.Role)
                .Include(u => u.TblProducts)
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            // If no user is found, return null
            if (user == null)
            {
                return null;
            }

            // Check the role and return the corresponding class
            if (user.Role.Role == "Employee")
            {
                return new Employee(user);
            }
            else if (user.Role.Role == "Farmer")
            {
                return new Farmer(user);
            }
            else
            {
                // Handle unknown roles or throw an exception if needed
                throw new InvalidOperationException("Unknown role: " + user.Role.Role);
            }
        }

        public async Task<string> checkRole(AgrEnergyDbContext context)
        {
            string? role = "";

            var user = context.TblUsers.Where(x => x.UserId == userID && x.Password == password).FirstOrDefault();

            role = context.TblRoles.Where(x => x.RoleId == user.RoleId).Select(x => x.Role).FirstOrDefault();

            if (role != null)
            {
                return role;
            }
            else throw new Exception("Impossible state exception(If you can see this then there is probably a hole in space time right now.)");
            
        }

        public static IUser getCurrentUser(HttpContext httpContext, AgrEnergyDbContext context)
        {
            var userJson = httpContext.Session.GetString("CurrentUser");

            if (string.IsNullOrEmpty(userJson))
            {
                return null;
            }
            else
            {
                // Deserialize JSON into either Farmer or Employee based on the stored user data
                if (userJson.Contains("Farmer")) // Check if the stored user data corresponds to a Farmer
                {
                    // Deserialize the JSON string into a dynamic object
                    dynamic userData = JsonConvert.DeserializeObject(userJson);

                    // Extract the user data from the dynamic object
                    IUser user = userData.User.ToObject<Farmer>();


                    return new Farmer(user, context);
                }
                else if (userJson.Contains("Employee")) // Check if the stored user data corresponds to an Employee
                {
                    // Deserialize the JSON string into a dynamic object
                    dynamic userData = JsonConvert.DeserializeObject(userJson);

                    // Extract the user data from the dynamic object
                    IUser user = userData.User.ToObject<Employee>();

                    return new Employee(user);
                }
                else
                {
                    // Handle unknown user type or invalid JSON format
                    throw new InvalidOperationException("Unknown user type or invalid JSON fo rmat.");
                }
            }
        }

        public async Task<IUser> loadUserDetails(AgrEnergyDbContext context)
        {
            string role = await checkRole(context);
            if (role == "Employee")
            {
                return new Employee(this);
            }else return new Farmer(this, context);
        }
    }
}
