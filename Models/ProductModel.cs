using System.ComponentModel.DataAnnotations;

namespace Product_Management_System.Models
{
    public class ProductModel
    {

        public int? ProductID { get; set; }
        [Required(ErrorMessage = "Product Name is Required to be filled")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Product Price is Required to be filled")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Product Price must be greater than 0.")]
        public double ProductPrice { get; set; }

        [Required(ErrorMessage = "Product Code is Required to be filled")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Product Description is Required to be filled")]
        public string Description { get; set; }

        [Required(ErrorMessage = "User Name is Required to be filled")]
        public int UserID { get; set; }

    }
    public class ProductDropDownModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }
}
