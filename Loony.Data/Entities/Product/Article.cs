using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.Product
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public int ArticleCode { get; set; }
        public string ArticleName { get; set; }
        public string DesignCode { get; set; }
        public string ColorCode { get; set; }
        public string Composition { get; set; }
        //public string CareInstructions { get; set; }

        public byte[] Image { get; set; }
        public string Description { get; set; }
        public string Supplier { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate { get; set; }
        public Guid CreatedBy { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModificationDate { get; set; }
        public Guid ModifiedBy { get; set; }

        public virtual ICollection<ProductChain> ProductChain { get; set; } = new HashSet<ProductChain>();
        public virtual ICollection<Article_DesignType> Article_DesignType { get; set; } = new HashSet<Article_DesignType>();

    }
}
