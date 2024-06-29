using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.Product
{
    public class DesignType
    {
        [Key]
        public int Id { get; set; }
        public string DesignTypeName { get; set; }

        public virtual ICollection<Article_DesignType> Article_DesignType { get; set; } = new HashSet<Article_DesignType>();

    }
}