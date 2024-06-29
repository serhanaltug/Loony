using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Loony.Data.Entities.System
{
    public class Menu_User
    {
        [Key]
        public int Id { get; set; }

        public int MenuId { get; set; }
        [ForeignKey("MenuId")]
        public Menu Menus { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User Users { get; set; }

        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool List { get; set; }
    }
}
