using Loony.Data.Entities.Product;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loony.Data.Entities.System
{
    public class File_Tag
    {
        [Key]
        public int Id { get; set; }

        public int ModelId { get; set; }
        [ForeignKey("FileId")]
        public File Files { get; set; }

        public int TagId { get; set; }
        [ForeignKey("TagId")]
        public Tag Tags { get; set; }
    }
}
