namespace PROGPOE_Part_2.Models
{
    public class EmployeeViewModel
    {
        public List<Farmer> lstFarmers { get; set; }
        public Dictionary<string, string> farmerProfiles { get; set; }
        public Employee currentEmployee { get; set; }


        public EmployeeViewModel() { }
    }
}
