using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.Product
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int Year { get; set; }
        public string Season { get; set; }
        public int ProductGroupId { get; set; }
        public virtual ProductGroup ProductGroup { get; set; }
        public string? Description { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate { get; set; }
        public Guid CreatedBy { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModificationDate { get; set; }
        public Guid ModifiedBy { get; set; }

        public virtual ICollection<ProductChain> ProductChain { get; set; } = new HashSet<ProductChain>();
        public virtual ICollection<ProductImage> ProductImage { get; set; } = new HashSet<ProductImage>();

    }
}
