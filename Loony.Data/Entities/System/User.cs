using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loony.Data.Entities.System
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string LastName { get; set; }
        [NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } }
        public bool IsAdmin { get; set; }
        public bool IsSuperUser { get; set; }
        public bool Status { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string Email { get; set; }
        [DataType(DataType.Password), StringLength(20, MinimumLength = 4)]
        //[Required(ErrorMessage = "Required field")]
        public string Password { get; set; }
        public string Phone { get; set; }
        public int Hit { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LastLoginIP { get; set; }
        public string Description { get; set; }
        public byte[] Avatar { get; set; }

        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BeginDate { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public virtual ICollection<Menu_User> Menu_User { get; set; } = new HashSet<Menu_User>();

    }
}
