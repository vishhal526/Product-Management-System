using System.ComponentModel.DataAnnotations;
namespace Product_Management_System.Models
{
    public class OrdersModel
    {
        public int? OrderID { get; set; }
        [Required(ErrorMessage = "OrderDate is required to be filled")]
        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "CustomerID is required to be filled")]
        public int CustomerID {  get; set; }
        [Required(ErrorMessage = "PaymentMode is required to be filled")]
        public string PaymentMode {  get; set; }
        [Required(ErrorMessage = "TotalAmount is required to be filled")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Amount must be greater than 0.")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "ShippingAddress is required to be filled")]
        public string ShippingAddress {  get; set; }
        [Required(ErrorMessage = "OrderNo is required to be filled")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Order Number must be greater than 0.")]
        public int UserID { get; set; }
    }
    public class OrderDropDownModel
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
