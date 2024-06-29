using System.ComponentModel.DataAnnotations;

namespace Loony.Web.Models
{
    public class AccessoryTypeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string AccessoryTypeName { get; set; }

    }
}
