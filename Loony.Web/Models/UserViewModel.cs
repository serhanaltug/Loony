using Loony.Data.Entities.System;
using System;
using System.Collections.Generic;

namespace Loony.Web.Models
{
    public class UserViewModel
    {
        //TODO: Normalde User yerine UserViewModel kullanmamız gerekiyor ve aynı ProfileViewModel de olduğu gibi verileri eşleştirip sonra göndermemiz gerekiyor.
    }

    public class AccessRightsViewModel
    {
        public Guid UserId { get; set; }
        public List<Menu> MainMenu { get; set; }
        public List<Menu> AdminMenu { get; set; }

        public List<Menu_User> UserMenu { get; set; }

    }
}
