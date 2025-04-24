using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Product_Management_System.Models
{
    public class CountryModel
{
    public int? CountryID { get; set; }
    [Required(ErrorMessage = "Country Name is Required")]
    [DisplayName("CountryName")]
    public string CountryName { get; set; }
    [Required(ErrorMessage = "Country Code is Required")]
    [DisplayName("CountryCode")]

    public string CountryCode { get; set; }
}
public class CountryDropDownModel
{
    public int CountryID { get; set; }
    public string CountryName { get; set; }

}
}
