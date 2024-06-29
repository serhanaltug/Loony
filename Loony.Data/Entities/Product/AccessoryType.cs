using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.Product
{
    public class AccessoryType
    {
        [Key]
        public int Id { get; set; }
        public string AccessoryTypeName { get; set; }

    }
}