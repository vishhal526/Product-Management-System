//codeshare.io/ar
using System.ComponentModel.DataAnnotations;
namespace Product_Management_System.Models
{
    public class BillModel
    {
        public int? BillID { get; set; }
        [Required(ErrorMessage= "BillNo is required to be filled")]
        public string BillNo{ get; set; }
        [Required(ErrorMessage = "BillDate is required to be filled")]
        [Range(typeof(DateTime), "1900-01-01", "2099-12-31", ErrorMessage = "BillDate must be between 01/01/1900 and 12/31/2099")]
        public DateTime BillDate { get; set; }
        [Required(ErrorMessage = "OrderID is required to be filled")]
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }
        [Required(ErrorMessage = "TotalAmount is required to be filled")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Amount must be greater than 0.")]
        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "Discount is required to be filled")]
        public decimal Discount { get; set; }
        [Required(ErrorMessage = "NetAmount is required to be filled")]
        [Range(0.01, double.MaxValue, ErrorMessage = "NetAmount must be greater than 0.")]
        public decimal NetAmount { get; set; }
        [Required(ErrorMessage = "UserID is required to be filled")]
        public int UserID { get; set; }

    }
}
