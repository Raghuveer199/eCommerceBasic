namespace ECommerce.Models
{
    public class product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Category { get; set; }
        public required decimal Price { get; set; }
        public required int Quantity { get; set; }
        public required string Description { get; set; }
        public required string ImageURL { get; set; }


        public product(int id, string name, string category, decimal price, int quantity, string description, string imageURL)
        {
            Id = id;
            Name = name;
            Category = category;
            Price = price;
            Quantity = quantity;
            Description = description;
            ImageURL = imageURL;
        }
    }

}
