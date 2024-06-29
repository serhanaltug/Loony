using Loony.Data;
using Loony.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Loony.Web.ViewComponents
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly DataContext db;

        public MainMenuViewComponent(DataContext dbContext)
        {
            db = dbContext;
        }

        public IViewComponentResult Invoke()
        {
            var menuList = db.Menu.Where(x => x.AdminMenu == false)
                    .Include(mu => mu.Menu_User).Where(x => x.Menu_User.Any(x => x.UserId == HttpContext.User.Id() && x.List == true));
            return View(menuList.ToList());
        }
    }
}
