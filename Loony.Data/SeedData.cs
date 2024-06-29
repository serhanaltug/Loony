using Loony.Data.Entities.System;
using Microsoft.EntityFrameworkCore;

namespace Loony.Data
{
    public static class SeedData
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>().HasData(
                new Language { Id = 1, ShortName = "EN", LongName = "English" },
                new Language { Id = 2, ShortName = "TR", LongName = "Türkçe" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = new System.Guid("68b5463b-d4ae-4a5f-9bac-86d6075add8c"), FirstName = "Loony", LastName = "Admin", Email = "admin@loonykids.com", Password = "1234", IsAdmin = true, Status = true, LanguageId = 1 }
            );

            modelBuilder.Entity<Menu>().HasData(
                new Menu { Id = 1, Name = "Dashboard", Controller = "Home", Action = "Index", AdminMenu = false, Icon = null, RelatedId = 0, Order = 1, Multi = 0, LinkId = 1, Everyone = true, SuperUser = true },
                new Menu { Id = 2, Name = "Collections", Controller = null, Action = null, AdminMenu = false, Icon = null, RelatedId = 0, Order = 2, Multi = 0, LinkId = 2, Everyone = true, SuperUser = true },
                new Menu { Id = 3, Name = "Products", Controller = "Product", Action = "Index", AdminMenu = false, Icon = "flaticon-business", RelatedId = 2, Order = 3, Multi = 0, LinkId = 2, Everyone = true, SuperUser = true },

                new Menu { Id = 4, Name = "System", Controller = null, Action = null, AdminMenu = true, Icon = "flaticon-settings", RelatedId = 0, Order = 1, Multi = 0, LinkId = 0, Everyone = false, SuperUser = true },
                new Menu { Id = 5, Name = "Users", Controller = "User", Action = "Index", AdminMenu = true, Icon = "flaticon-users", RelatedId = 4, Order = 2, Multi = 0, LinkId = 0, Everyone = false, SuperUser = false },
                new Menu { Id = 6, Name = "Translations", Controller = "Translation", Action = "Index", AdminMenu = true, Icon = "flaticon-earth-globe", RelatedId = 4, Order = 3, Multi = 0, LinkId = 0, Everyone = false, SuperUser = true },
                new Menu { Id = 7, Name = "Menu", Controller = "Menu", Action = "Index", AdminMenu = true, Icon = "flaticon-list", RelatedId = 4, Order = 4, Multi = 0, LinkId = 0, Everyone = false, SuperUser = true },
                new Menu { Id = 8, Name = "AccessRights", Controller = "AccessRights", Action = "Index", AdminMenu = true, Icon = "flaticon-lock", RelatedId = 4, Order = 5, Multi = 0, LinkId = 0, Everyone = false, SuperUser = true }
            );

            modelBuilder.Entity<Menu_User>().HasData(
                new Menu_User { Id = 1, MenuId = 1, UserId = new System.Guid("68b5463b-d4ae-4a5f-9bac-86d6075add8c"), Create = true, Read = true, Update = true, Delete = true, List = true },
                new Menu_User { Id = 2, MenuId = 2, UserId = new System.Guid("68b5463b-d4ae-4a5f-9bac-86d6075add8c"), Create = true, Read = true, Update = true, Delete = true, List = true },
                new Menu_User { Id = 3, MenuId = 3, UserId = new System.Guid("68b5463b-d4ae-4a5f-9bac-86d6075add8c"), Create = true, Read = true, Update = true, Delete = true, List = true },
                new Menu_User { Id = 4, MenuId = 4, UserId = new System.Guid("68b5463b-d4ae-4a5f-9bac-86d6075add8c"), Create = true, Read = true, Update = true, Delete = true, List = true },
                new Menu_User { Id = 5, MenuId = 5, UserId = new System.Guid("68b5463b-d4ae-4a5f-9bac-86d6075add8c"), Create = true, Read = true, Update = true, Delete = true, List = true },
                new Menu_User { Id = 6, MenuId = 6, UserId = new System.Guid("68b5463b-d4ae-4a5f-9bac-86d6075add8c"), Create = true, Read = true, Update = true, Delete = true, List = true },
                new Menu_User { Id = 7, MenuId = 7, UserId = new System.Guid("68b5463b-d4ae-4a5f-9bac-86d6075add8c"), Create = true, Read = true, Update = true, Delete = true, List = true },
                new Menu_User { Id = 8, MenuId = 8, UserId = new System.Guid("68b5463b-d4ae-4a5f-9bac-86d6075add8c"), Create = true, Read = true, Update = true, Delete = true, List = true }
            );

        }
    }
}
