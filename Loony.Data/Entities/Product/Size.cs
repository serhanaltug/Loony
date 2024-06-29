using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.Product
{
    public class Size
    {
        [Key]
        public int Id { get; set; }
        public string ProductSize { get; set; }
        public string ProductLine { get; set; }
    }
}
