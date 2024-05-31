using Part2Library.Models;
using Newtonsoft.Json;

namespace PROGPOE_Part_2.Models
{
    public class Farmer : IUser
    {
        [JsonIgnore]
        public List<Product> lstProducts = new List<Product>();

        public string userID { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public Farmer() { }

        [JsonConstructor]
        public Farmer(string userID, string userName, string email, string password, string Role)
        {
            this.userID = userID;
            this.userName = userName;
            this.email = email;
            this.password = password;
        }

        public Farmer(TblUser user)
        {
            userID = user.UserId;
            userName = user.Username;
            email = user.Email;
            password = user.Password;

            var lstTblProducts = user.TblProducts.ToList();
            lstProducts = Product.mapToProducts(lstTblProducts);
        }

        public Farmer(IUser user, AgrEnergyDbContext context)
        {
            userID = user.userID;
            userName = user.userName;
            email = user.email;
            password = user.password;

            var products = context.TblProducts.Where(x => x.UserId == userID).ToList();

            lstProducts = Product.mapToProducts(products);
        }

        public void addProduct(Product product)
        {
            lstProducts.Add(product);
        }

        public static List<Farmer> mapToFarmer(List<TblUser> tblFarmers)
        {
            return tblFarmers.Select(farmer => new Farmer(
            farmer.UserId,
            farmer.Username,
            farmer.Email,
            farmer.Password,
            farmer.RoleId
            )).ToList();
        }

        public int getNumProducts(AgrEnergyDbContext context)
        {
            int total = 0;

            var numProducts = context.TblProducts.Where(x => x.UserId == userID).Count();

            total += numProducts;

            return total;
        }
    }
}
