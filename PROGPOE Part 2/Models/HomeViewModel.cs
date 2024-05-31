using Part2Library.Models;

namespace PROGPOE_Part_2.Models
{
    public class HomeViewModel
    {
        public List<TblCategory> lstCategories { get; set; }
        public Dictionary<string, string> ImageUrls { get; set; }

        public HomeViewModel() 
        {
            
        }

    }
}
