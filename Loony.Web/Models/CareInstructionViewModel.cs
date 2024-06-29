using System.ComponentModel.DataAnnotations;

namespace Loony.Web.Models
{
    public class CareInstructionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Name { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public byte Image { get; set; }

    }
}
