using Loony.Data.Entities.Product;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Loony.Web.Models
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        public Product Product { get; set; }

        public IEnumerable<int> Images { get; set; }
        public int BaseImageId { get; set; }

        public MultiSelectList ModelSelectList { get; set; }
        public SelectList ProductGroupSelectList { get; set; }
    }
    public class ProductImageViewModel
    {
        public ProductImage ProductImage { get; set; }
    }

}
