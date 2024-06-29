using System.ComponentModel.DataAnnotations;

namespace Loony.Web.Models
{
    public class ProductGroupViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string GroupName { get; set; }

        [Required(ErrorMessage = "Required field")]
        public int Order { get; set; }

        public string Description { get; set; }

    }
}
