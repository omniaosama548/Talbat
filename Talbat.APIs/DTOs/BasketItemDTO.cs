using System.ComponentModel.DataAnnotations;

namespace Talbat.APIs.DTOs
{
    public class BasketItemDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string productName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [Range(0.1,double.MaxValue,ErrorMessage ="Price Can Not Be Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(0.1, int.MaxValue, ErrorMessage = "Quantity Can Not Be Zero")]
        public int Quantity { get; set; }
    }
}