using Loony.Data.Entities.Product;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Loony.Web.Models
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        public Article Article { get; set; }

        public byte[] Image { get; set; }

        public string[] DesignTypes { get; set; }

        public List<SelectListItem> DesignTypeSelectList { get; set; }


    }
}