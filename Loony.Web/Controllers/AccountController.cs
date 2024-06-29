using Loony.Data;
using Loony.Tools;
using Loony.Web.Extensions;
using Loony.Web.Filters;
using Loony.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Loony.Web.Controllers
{
    [ExceptionFilter]
    public class AccountController : Controller
    {
        private readonly DataContext db;

        public AccountController(DataContext dbContext)
        {
            db = dbContext;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginUser = db.Users.Include(x => x.Language).FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower() && x.Password == user.Password && x.Status == true);
                if (loginUser != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginUser.FullName),
                        new Claim("Id", loginUser.Id.ToString()),
                        new Claim(ClaimTypes.Email, loginUser.Email),
                        new Claim("Language", loginUser.Language.ShortName),
                        new Claim("IsAdmin", loginUser.IsAdmin.ToString()),
                        new Claim("IsSuperUser", loginUser.IsSuperUser.ToString()),
                        new Claim("OriginId", String.Empty)
                    };
                    var identity = new ClaimsIdentity(claims, "UserIdentity");
                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        IsPersistent = user.RememberMe,
                        ExpiresUtc = DateTime.UtcNow.AddMonths(1)
                    };
                    await HttpContext.SignInAsync(new ClaimsPrincipal(identity), authProperties);

                    loginUser.Hit += 1;
                    loginUser.LastLoginDate = DateTime.Now;
                    db.SaveChanges();

                    //if (!Url.IsLocalUrl(returnUrl))
                    //{
                    //    returnUrl = Url.Content("~/");
                    //}

                    //return LocalRedirect(returnUrl);
                    return Ok();

                }
                else
                {
                    ModelState.AddModelError("InvalidUser", "Email or password not found or matched");
                    return BadRequest();
                }
            }
            else
            {

            }
            //hata mesajı gönderilecek.
            return BadRequest();
        }

        public async Task<IActionResult> LoginAs(Guid id, string token)
        {
            var loginUser = db.Users.Include(x => x.Language).FirstOrDefault(x => x.Id == id && x.Status == true);
            if (loginUser != null && Security.ValidateToken(id.ToString(), token))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginUser.FullName),
                    new Claim("Id", loginUser.Id.ToString()),
                    new Claim(ClaimTypes.Email, loginUser.Email),
                    new Claim("Language", loginUser.Language.ShortName),
                    new Claim("IsAdmin", loginUser.IsAdmin.ToString()),
                    new Claim("IsSuperUser", loginUser.IsSuperUser.ToString()),
                    new Claim("OriginId", HttpContext.User.Id().ToString())
                };
                var identity = new ClaimsIdentity(claims, "UserIdentity");
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                };
                await HttpContext.SignInAsync(new ClaimsPrincipal(identity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Edit", "User", new { id = id.ToString() });
        }

        public IActionResult Avatar(Guid guid)
        {
            var avatar = Convert.ToBase64String(db.Users.Find(guid).Avatar);
            return PartialView(avatar);
        }

        [Route("/AccessDenied")]
        [Route("/AccessDenied/{title}")]
        public ActionResult AccessDenied(string title)
        {
            return View("NoAccess", title);
        }
    }
}
