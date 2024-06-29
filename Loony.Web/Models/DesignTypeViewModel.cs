using System.ComponentModel.DataAnnotations;

namespace Loony.Web.Models
{
    public class DesignTypeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string DesignTypeName { get; set; }

    }
}
