namespace Product_Management_System.Models
{
    public class DashboardModel
    {
        public List<DashboardCounts> Counts { get; set; }
        public List<RecentOrder> RecentOrders { get; set; }
        public List<RecentProduct> RecentProducts { get; set; }
        public List<TopCustomer> TopCustomers { get; set; }
        public List<TopSellingProduct> TopSellingProducts { get; set; }
        public List<QuickLinks> NavigationLinks { get; set; }
    }
    public class DashboardCounts
    {
        public string Metric { get; set; }
        public int Value { get; set; }
    }

    public class RecentOrder
    {
        public int OrderID { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
    }

    public class RecentProduct
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
    }

    public class TopCustomer
    {
        public string CustomerName { get; set; }
        public int TotalOrders { get; set; }
        public string Email { get; set; }
    }

    public class TopSellingProduct
    {
        public string ProductName { get; set; }
        public int TotalSoldQuantity { get; set; }
        public string ProductCode { get; set; }
    }

    public class QuickLinks
    {
        public string ActionMethodName { get; set; }
        public string ControllerName { get; set; }
        public string LinkName { get; set; }
    }
}
