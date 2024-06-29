using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.Product
{
    public class ProductGroup
    {
        [Key]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public int Order { get; set; }
        public string Description { get; set; }
    }
}
