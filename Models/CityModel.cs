using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Product_Management_System.Models
{
    public class CityModel
    {
        public int? CityID { get; set; }
        public string CityName { get; set; }
        public int CountryID { get; set; }
        public int StateID { get; set; }
        public string? CountryName { get; set; }
        public string? StateName { get; set; }
        public string CityCode { get; set; }
    }

}
