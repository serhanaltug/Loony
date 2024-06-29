using Microsoft.AspNetCore.Mvc.Filters;

namespace Loony.Web.Filters
{
    public class ExceptionFilter : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;
            //MailSender.SendEmail("serhan.altug@websiyon.com", $"User: {filterContext.HttpContext.User.Identity.Name}<br>Date: {DateTime.Now}<br>ErrorMessage: {ex}", "Loony Kids Error");
        }
    }
}
