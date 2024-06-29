using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.Product
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        public Guid ProductId { get; set; }
        public int ColorCode { get; set; }
        public byte[] Image { get; set; }
        public bool Base { get; set; }
        public int Order { get; set; }
        public string Tags { get; set; }
    }
}
