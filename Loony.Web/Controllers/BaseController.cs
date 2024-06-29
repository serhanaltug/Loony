using Loony.Data;
using Loony.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Loony.Web.Controllers
{
    [Authorize, AuthorizationFilter, ExceptionFilter]
    public class BaseController : Controller
    {
        private DataContext _db;
        private IWebHostEnvironment _env;

        protected DataContext db => _db ??= HttpContext.RequestServices.GetService<DataContext>();
        protected IWebHostEnvironment env => _env ??= HttpContext.RequestServices.GetService<IWebHostEnvironment>();
    }
}
