using Loony.Web.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Loony.Web.Filters
{
    public class LogFilter : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var userid = context.HttpContext.User.Id();
            var user = context.HttpContext.User.Identity.Name;
            var controller = context.RouteData.Values["controller"].ToString();
            var action = context.RouteData.Values["action"].ToString();


            //var form = context.HttpContext.Request.Form;
            //var formData = form.Keys.Cast<string>().ToDictionary(k => k, k => form[k]);
            //var jsonFormData = JsonConvert.SerializeObject(formData);

            //var queryString = context.HttpContext.Request.QueryString;
            //var queryStringData = queryString.Keys.Cast<string>().ToDictionary(k => k, k => queryString[k]);
            //var jsonQueryStringData = JsonConvert.SerializeObject(queryString);
            //TODO: QueryString verisini alırken Keys hata verdi.

            //var db = context.HttpContext.RequestServices.GetService<DataContext>();
            //db.Logs.Add(new Log()
            //{
            //    UserId = userid,
            //    Username = user,
            //    Controller = controller,
            //    Action = action,
            //    Description = description
            //});
            //db.SaveChanges();
        }
    }
}
