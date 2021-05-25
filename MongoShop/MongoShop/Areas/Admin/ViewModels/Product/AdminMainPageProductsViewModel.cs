namespace MongoShop.Areas.Admin.ViewModels.Product
{
    public class SelectMainPageProductsViewModel
    {
        public string ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Size { get; set; }
        public int StockQuantity { get; set; }
        public string SubCategory { get; set; }
        public string Category { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
