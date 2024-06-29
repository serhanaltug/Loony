using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.Product
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string TagName { get; set; }
    }
}
