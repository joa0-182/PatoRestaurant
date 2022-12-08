namespace PatoRestaurant.ViewModels
{
    public class Menu
    {
        public Category Category { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}