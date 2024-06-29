using Loony.Data;
using Loony.Tools;
using Loony.Web.Extensions;
using Loony.Web.Filters;
using Loony.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Security.Claims;

namespace Loony.Web.Controllers
{
    [ExceptionFilter]
    public class HomeController : Controller
    {
        private IMemoryCache _memoryCache;
        private readonly DataContext db;
        private IWebHostEnvironment _env;

        public HomeController(IMemoryCache memoryCache, DataContext dbContext, IWebHostEnvironment env)
        {
            _memoryCache = memoryCache;
            db = dbContext;
            _env = env;

        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }
        public IActionResult _Test()
        {
            Thread.Sleep(2000);
            return PartialView();
        }

        public IActionResult _GetArticles(string article)
        {
            if (article.Length > 0)
            {
                var data = db.Articles.Where(x => x.ArticleName.ToLower().Contains(article.ToLower())).ToList();
                return PartialView(data);
            }
            return PartialView();
        }

        public IActionResult _AnOtherTest() => PartialView();


        public IActionResult Avatar(Guid id)
        {
            byte[] file = db.Users.Find(id).Avatar;
            if (file != null)
            {
                string mimeType = "image/png";
                return File(file, mimeType);
            }

            return null;
        }

        public IActionResult Image(int id, string table)
        {
            if (string.IsNullOrEmpty(table) || id == 0) return Content("Image source not defined.");

            try
            {
                Type entityType = Type.GetType(string.Format("Loony.Data.Entities.{0},{1}", table, "Loony.Data"));
                var entity = db.Find(entityType, id);
                var propertyInfo = entity.GetType().GetProperty("Image");
                var image = propertyInfo.GetValue(entity, null) as byte[];
                string mimeType = "image/png";

                if (image != null)
                {
                    return File(image, mimeType);
                }
                var path = Path.Combine(_env.ContentRootPath, "wwwroot", "img", "noimage.png");
                var file = System.IO.File.ReadAllBytes(path);

                return File(file, mimeType);
            }
            catch (Exception ex)
            {
                return Content($"File error. ({ex})");
            }
        }

        public IActionResult Thumbnail(int id, string table, int width = 300, int height = 300)
        {
            if (string.IsNullOrEmpty(table) || id == 0) return Content("Image source not defined.");

            try
            {
                Type entityType = Type.GetType(string.Format("Loony.Data.Entities.{0},{1}", table, "Loony.Data"));
                var entity = db.Find(entityType, id);
                var propertyInfo = entity.GetType().GetProperty("Image");
                var image = propertyInfo.GetValue(entity, null) as byte[];
                string mimeType = "image/png";

                if (image == null)
                {
                    var path = Path.Combine(_env.ContentRootPath, "wwwroot", "img", "noimage.png");
                    image = System.IO.File.ReadAllBytes(path);
                }

                var img = ImageEditor.byteArrayToImage(image);
                img = ImageEditor.SquareToLongEdge(img);
                img = ImageEditor.FixedSize(img, width, height);
                image = ImageEditor.imageToByteArray(img);

                return File(image, mimeType);
            }
            catch (Exception ex)
            {
                return Content($"File error. ({ex})");
            }
        }

        [Authorize]
        public async Task<IActionResult> ChangeLanguage(int languageId, string returnUrl)
        {
            var user = db.Users.Find(User.Id());
            var language = db.Languages.Find(languageId).ShortName;
            user.LanguageId = languageId;
            db.SaveChanges();

            if (HttpContext.User.Identity is ClaimsIdentity identity)
            {
                identity.RemoveClaim(identity.FindFirst("Language"));
                identity.AddClaim(new Claim("Language", language));
                await HttpContext.SignOutAsync();
                await HttpContext.SignInAsync(new ClaimsPrincipal(HttpContext.User.Identity));
            }

            _memoryCache = new MemoryCache(new MemoryCacheOptions());

            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.Content("~/");

            return LocalRedirect(returnUrl);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
