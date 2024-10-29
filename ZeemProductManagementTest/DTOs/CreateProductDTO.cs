using System.ComponentModel.DataAnnotations;

namespace ZeemProductManagementTest.DTOs
{
    public class CreateProductDTO
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, 99999999.999999999)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 99999999)]
        public int Stock { get; set; }
    }
}
