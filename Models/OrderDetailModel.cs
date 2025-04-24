using System.ComponentModel.DataAnnotations;
namespace Product_Management_System.Models
{
    public class OrderDetailModel
    {
        public int? OrderDetailID { get; set; }
        [Required(ErrorMessage = "OrderID is required to be filled")]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "ProductID is required to be filled")]
        public int ProductID { get; set; }
        [Required(ErrorMessage = "Quantity is required to be filled")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Amount is required to be filled")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "TotalAmount is required to be filled")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "UserID is required to be filled")]
        public int UserID{ get; set; }

    }
}
