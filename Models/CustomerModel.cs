using System.ComponentModel.DataAnnotations;

namespace Product_Management_System.Models
{
    public class CustomerModel
    {
        public int? CustomerID { get; set; }

        [Required(ErrorMessage= "Customer Name is Required to be filled")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Customer Home Address is Required to be filled")]
        public string HomeAddress { get; set; }

        [Required(ErrorMessage = "Customer Email is Required to be filled")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Customer Mobile Number is Required to be filled")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Customer Home Address is Required to be filled")]
        public string GSTNO  { get; set; }

        [Required(ErrorMessage = "Customer City Name is Required to be filled")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "Customer Pin code is Required to be filled")]
        public string PinCode { get; set; }

        [Required(ErrorMessage = "Net Amount field is Required to be filled")]
        [Range(0.01, double.MaxValue, ErrorMessage = "NetAmount must be greater than 0.")]
        public decimal NetAmount { get; set; }

        [Required(ErrorMessage = "Customer User Id is Required to be filled")]
        public int UserID { get; set; }

    }
    public class CustomerDropDownModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}
