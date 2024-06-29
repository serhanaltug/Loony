using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loony.Data.Entities.System
{
    public class FileGroup
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Zorunlu alan"), StringLength(50)]
        public string FileGroupName { get; set; }

        public int? RelatedGrupID { get; set; }
        [ForeignKey("RelatedGrupID")]
        public virtual FileGroup RelatedGroup { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public virtual ICollection<File> Files { get; set; } = new HashSet<File>();
    }
}
