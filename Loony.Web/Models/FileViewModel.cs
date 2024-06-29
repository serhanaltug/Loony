using Loony.Data.Entities.System;
using System.ComponentModel.DataAnnotations;

namespace Loony.Web.Models
{
    public class FileViewModel
    {
        [Key]
        public Guid Id { get; set; }

        //public string RelatedTable { get; set; }
        //public Guid? RelatedGuid { get; set; }
        //public int? RelatedId { get; set; }

        [Required, StringLength(255)]
        public string FileName { get; set; }
        public int? FileType { get; set; }
        [StringLength(100)]
        public string MIMEType { get; set; }

        //[Required]
        //public byte[] FileContent { get; set; }


        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreationDate { get; set; }
        public Guid CreatedBy { get; set; }

        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? ModificationDate { get; set; }
        //public Guid ModifiedBy { get; set; }

        public int? FileGroupId { get; set; }
        public virtual FileGroup FileGroup { get; set; }

        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? BeginDate { get; set; }

        //[DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? EndDate { get; set; }

        //public virtual ICollection<File_Tag> File_Tag { get; set; } = new HashSet<File_Tag>();

    }
}
