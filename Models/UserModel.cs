using System.ComponentModel.DataAnnotations;
namespace Product_Management_System.Models
{
    public class UserModel
    {

        public int? UserID { get; set; }
        [Required(ErrorMessage = "UserName is Required to be filled")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is Required to be filled")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required to be filled")]
        public string Password { get; set; }
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        [Required(ErrorMessage = "MobileNo is Required to be filled")]
        public string MobileNo { get; set; }
        [Required(ErrorMessage = "Address is Required to be filled")]
        public string Address { get; set; }
        [Required(ErrorMessage = "isActive field is Required to be filled")]
        public bool? isActive { get; set; }

    }
    public class UserDropDownModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }

    public class UserLoginModel 
    {
        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }

    public class UserRegisterModel
    {
        public int? UserID { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Mobile number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be exactly 10 digits.")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        public bool? isActive { get; set; }
    }
}
