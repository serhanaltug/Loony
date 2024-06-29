using Loony.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Loony.Web.Controllers
{
    public class LicenseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Activate(string activationKey)
        {
            if (!String.IsNullOrEmpty(activationKey))
                LicenseManager.Activate(activationKey);
            return RedirectToAction("Index");
        }

        //public string CreateLicense(string Id)
        //{
        //    var result = LicenseManager.CreateToken(Id);
        //    return result;
        //}


    }
}
