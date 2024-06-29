using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Loony.Data.Entities.System
{
    public class Menu
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool AdminMenu { get; set; }
        public bool SuperUser { get; set; }
        public bool Everyone { get; set; }
        public string Icon { get; set; }
        public int RelatedId { get; set; }
        public int Order { get; set; }
        public int Multi { get; set; }
        public int LinkId { get; set; }

        public virtual ICollection<Menu_User> Menu_User { get; set; } = new HashSet<Menu_User>();


    }
}
