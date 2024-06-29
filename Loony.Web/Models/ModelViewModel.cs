using Loony.Data.Entities.Product;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Loony.Web.Models
{
    public class ModelViewModel
    {
        public int Id { get; set; }

        public Model ProductModel { get; set; }

        public byte[] Image { get; set; }

        public string[] Tags { get; set; }
        public List<SelectListItem> TagSelectList { get; set; }

        public SelectList ModelGroupSelectList { get; set; }
    }
}
