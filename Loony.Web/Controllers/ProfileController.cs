using Loony.Tools;
using Loony.Web.Extensions;
using Loony.Web.Filters;
using Loony.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Loony.Web.Controllers
{
    public class ProfileController : BaseController
    {

        public async Task<IActionResult> Index()
        {
            var user = await db.Users.FindAsync(User.Id());

            if (user == null)
            {
                return NotFound();
            }

            var model = new ProfileViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Avatar = user.Avatar,
                LastLoginDate = user.LastLoginDate,
                LastLoginIP = user.LastLoginIP,
                Hit = user.Hit,
                LanguageId = user.LanguageId,
                Languages = new SelectList(db.Languages.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.LongName }), "Value", "Text", user.LanguageId)
            };

            ViewBag.Genders = new SelectList(
                Enum.GetValues(typeof(ProfileViewModel.Gender)).Cast<ProfileViewModel.Gender>()
                .Select(x => new SelectListItem() { Value = x.ToString(), Text = x.ToString() }), "Value", "Text");

            return View(model);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [FromForm] ProfileViewModel model, IFormFile file)
        {
            if (id != model.Id) return NotFound();
            if (!ModelState.IsValid) return View(model);

            var entity = db.Users.Find(id);

            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Email = model.Email;
            entity.LanguageId = model.LanguageId;
            entity.Phone = model.Phone;

            if (!string.IsNullOrEmpty(model.Password1) && model.Password1 == model.Password2)
                entity.Password = model.Password1;

            //TODO: Dil değiştiyse UserController daki ChangeLanguage metodundaki kodlar burada da kullanılabilir.

            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var TempImage = Image.FromStream(ms);

                    int OrginalWidth, OrginalHeight;

                    int TempHeight = TempImage.Height;
                    int TempWidth = TempImage.Width;

                    if (TempWidth > 500)
                        OrginalWidth = 500;
                    else
                        OrginalWidth = TempWidth;

                    OrginalHeight = (int)(OrginalWidth * TempHeight / (double)TempWidth);

                    TempImage.Dispose();

                    var img = ImageEditor.FixedSize(Image.FromStream(ms), OrginalWidth, OrginalHeight);
                    img = ImageEditor.SquareToShortEdge(img);
                    entity.Avatar = ImageEditor.imageToByteArray(img);
                }
            }

            TempData["Message"] = "msgSaved";

            db.Update(entity);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
