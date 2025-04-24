using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Product_Management_System.Models
{
    public class StateModel
    {
        public int? StateID { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public int CountryID { get; set; }
        public string? CountryName { get; set; }

    }
    public class StateDropDownModel
    {
        public int StateID { get; set; }
        public string StateName { get; set; }

    }
}
