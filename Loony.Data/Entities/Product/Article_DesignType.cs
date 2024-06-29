using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loony.Data.Entities.Product
{
    public class Article_DesignType
    {
        [Key]
        public int Id { get; set; }

        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public Article Articles { get; set; }

        public int DesignTypeId { get; set; }
        [ForeignKey("DesignTypeId")]
        public DesignType DesignTypes { get; set; }
    }
}
