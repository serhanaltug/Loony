using System.ComponentModel.DataAnnotations;

namespace Loony.Web.Models
{
    public class LanguageViewModel
    {
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
}
