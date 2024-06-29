using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loony.Data.Entities.Product
{
    public class ProductChain
    {
        [Key]
        public int Id { get; set; }

        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int? ModelId { get; set; }
        [ForeignKey("ModelId")]
        public Model Model { get; set; }

        public int? ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Article { get; set; }

        public int? AccessoryId { get; set; }
        [ForeignKey("AccessoryId")]
        public Accessory Accessory { get; set; }
    }
}
