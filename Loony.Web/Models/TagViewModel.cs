using System.ComponentModel.DataAnnotations;

namespace Loony.Web.Models
{
    public class TagViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string TagName { get; set; }

    }
}
