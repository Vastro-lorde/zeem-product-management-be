using System.ComponentModel.DataAnnotations;

namespace ZeemProductManagementTest.DTOs
{
    public class UpdateProductDTO
    {
        [MaxLength(150)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(0, 99999999.999999999)]
        public decimal? Price { get; set; }

        [Range(0, 99999999)]
        public int? Stock { get; set; }
    }
}
