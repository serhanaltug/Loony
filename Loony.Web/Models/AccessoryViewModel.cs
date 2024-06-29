using Loony.Data.Entities.Product;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Loony.Web.Models
{
    public class AccessoryViewModel
    {
        public int Id { get; set; }

        public Accessory Accessory { get; set; }

        public byte[] Image { get; set; }

        public SelectList AccessoryTypeSelectList { get; set; }
    }
}