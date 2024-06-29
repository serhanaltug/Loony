using Loony.Data;
using Loony.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Loony.Web.Filters
{
    public class AuthorizationFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.IsAdmin())
            {

                var result = false;

                context.HttpContext.Items["Create"] = false;
                context.HttpContext.Items["Update"] = false;
                context.HttpContext.Items["Delete"] = false;

                var db = context.HttpContext.RequestServices.GetRequiredService<DataContext>();

                var userId = context.HttpContext.User.Id();
                var controller = context.RouteData.Values["controller"].ToString();
                var action = context.RouteData.Values["action"].ToString();

                int menuID = 0;

                var menu = db.Menu.FirstOrDefault(x => x.Controller == controller && x.Action == action);
                if (menu != null)
                {
                    menuID = menu.Id;
                }
                else
                {
                    var menuIndex = db.Menu.FirstOrDefault(x => x.Controller == controller && x.Action == "Index");
                    if (menuIndex != null)
                    {
                        menuID = menuIndex.Id;
                    }
                }

                if (menuID > 0)
                {
                    var access = db.Menu_User.FirstOrDefault(x => x.UserId == userId && x.MenuId == menuID);
                    if (access != null)
                    {
                        if ((action == "Index" || action == "_List") && (access.List || access.Read)) result = true;

                        if ((action == "New" || action == "_New") && access.Create) result = true;
                        if ((action == "Edit" || action == "_Edit") && access.Update) result = true;
                        if ((action == "Delete" || action == "_Delete") && access.Delete) result = true;

                        if (access.Create) context.HttpContext.Items["Create"] = true;
                        if (access.Update) context.HttpContext.Items["Update"] = true;
                        if (access.Delete) context.HttpContext.Items["Delete"] = true;
                    }
                }

                if (result == false)
                    context.Result = new RedirectResult("~/AccessDenied/" + controller);
            }
            else
            {
                context.HttpContext.Items["Create"] = true;
                context.HttpContext.Items["Update"] = true;
                context.HttpContext.Items["Delete"] = true;
            }

        }
    }
}
