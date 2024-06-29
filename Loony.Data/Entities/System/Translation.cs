using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.System
{
    public class Translation
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string Key { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string EN { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string TR { get; set; }

        [Required(ErrorMessage = "Required field")]
        public int Type { get; set; } = 1;

        public string Group { get; set; }

    }

    public enum TextType
    {
        Static = 1,
        Dynamic = 2
    }

}
