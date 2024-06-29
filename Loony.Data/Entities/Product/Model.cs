using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.Product
{
    public class Model
    {
        [Key]
        public int Id { get; set; }
        public int ModelCode { get; set; }
        public string ModelName { get; set; }
        public int Year { get; set; }
        public string Season { get; set; }

        public byte[] Image { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate { get; set; }
        public Guid CreatedBy { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModificationDate { get; set; }
        public Guid ModifiedBy { get; set; }

        public int ModelGroupId { get; set; }
        public virtual ProductGroup ModelGroup { get; set; }

        public virtual ICollection<ProductChain> ProductChain { get; set; } = new HashSet<ProductChain>();
        public virtual ICollection<Model_Tag> Model_Tag { get; set; } = new HashSet<Model_Tag>();

    }
}
