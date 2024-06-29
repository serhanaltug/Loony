using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loony.Data.Entities.Product
{
    public class Model_Tag
    {
        [Key]
        public int Id { get; set; }

        public int ModelId { get; set; }
        [ForeignKey("ModelId")]
        public Model Models { get; set; }

        public int TagId { get; set; }
        [ForeignKey("TagId")]
        public Tag Tags { get; set; }
    }
}
