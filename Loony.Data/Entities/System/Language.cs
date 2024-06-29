using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.System
{
    public class Language
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string LongName { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string ShortName { get; set; }
    }
}
