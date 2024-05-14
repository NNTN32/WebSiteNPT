using System.ComponentModel.DataAnnotations;

namespace WebShopNPT.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Range(0.001, 100.000)]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public float Discount { get; set; }
        public int SKU { get; set; }
        public string? ImageUrl { get; set; }
        public List<ProductImage>? Images { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int BrandId { get; set; }
        public Brand? Brand { get; set; }

        internal void AddAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
