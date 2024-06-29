using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.Product
{
    public class Accessory
    {
        [Key]
        public int Id { get; set; }
        public int AccessoryCode { get; set; }
        public string AccessoryName { get; set; }

        public byte[] Image { get; set; }
        public string Description { get; set; }
        public string Supplier { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate { get; set; }
        public Guid CreatedBy { get; set; }
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ModificationDate { get; set; }
        public Guid ModifiedBy { get; set; }

        public int AccessoryTypeId { get; set; }
        public virtual AccessoryType AccessoryType { get; set; }


        public virtual ICollection<ProductChain> ProductChain { get; set; } = new HashSet<ProductChain>();


    }
}
