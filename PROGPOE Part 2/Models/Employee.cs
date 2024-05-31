using Part2Library.Models;
using Newtonsoft.Json;

namespace PROGPOE_Part_2.Models
{
    public class Employee : IUser
    {
        public string userID { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        [JsonConstructor]
        public Employee(string userID, string userName, string email, string password)
        {
            this.userID = userID;
            this.userName = userName;
            this.email = email;
            this.password = password;
        }

        public Employee(TblUser user) 
        {
            userID = user.UserId;
            userName = user.Username;
            email = user.Email;
            password = user.Password;
        }
        
        public Employee(IUser user)
        {
            userID = user.userID;
            userName = user.userName;
            email = user.email;
            password = user.password;
        }
    }
}
