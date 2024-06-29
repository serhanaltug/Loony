using Loony.Data.Entities.System;
using Loony.Tools;
using Loony.Web.Extensions;
using Loony.Web.Filters;
using Loony.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Loony.Web.Controllers
{
    public class UserController : BaseController
    {

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult _List(string filters, string search, string order = "Id", string sortdir = "desc", int page = 1)
        {
            var model = new ListViewModel<User>(filters, search, order, sortdir);

            IQueryable<User> users = db.Users;

            if (search != null && search.Length > 0)
            {
                users = users.Where(x =>
                x.FirstName.ToLower().Contains(search.ToLower()) ||
                x.LastName.ToLower().Contains(search.ToLower()) ||
                x.Email.ToLower().Contains(search.ToLower())
                );
            }

            if (filters != null && filters.Length > 0)
            {
                filters = HttpUtility.UrlDecode(filters);
                var f = JsonSerializer.Deserialize<string[]>(filters);

                var filter = new List<Filter>();

                foreach (var item in f)
                {
                    var i = item.Split(':');
                    object v = i[1];

                    if (int.TryParse(v.ToString(), out _))
                        v = Convert.ToInt32(v);
                    else if (v.ToString() == "true")
                        v = Convert.ToBoolean(true);
                    else if (v.ToString() == "false")
                        v = Convert.ToBoolean(false);

                    filter.Add(new Filter { PropertyName = i[0], Operation = Op.Equals, Value = v });
                }

                users = users.Where(filter);
            }

            if (order != null)
            {
                users = users.OrderBy(order, sortdir.Contains("desc"));
            }

            model.Page.DataCount = users.Count();
            model.Page.SelectedPage = page;
            model.Data = PaginatedList<User>.CreateAsync(users.AsNoTracking(), page, model.Page.PageSize).Result;

            return PartialView(model);
        }

        public IActionResult New()
        {
            return View();
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([FromForm] User user, IFormFile file)
        {
            //if (!ModelState.IsValid) return View(user);

            var checkEmail = db.Users.FirstOrDefault(x => x.Email.ToLower() == user.Email.ToLower());
            if (checkEmail != null)
            {
                ModelState.AddModelError("InvalidEmail", "InvalidEmail");
                return View(user);
            }

            var entity = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin,
                IsSuperUser = user.IsSuperUser,
                Status = true,
                Email = user.Email,
                Password = Security.CreatePassword(6),
                Phone = user.Phone,
                LanguageId = 1
            };

            var img = ImageEditor.CreateAvatar(user.FirstName.Substring(0, 1) + user.LastName.Substring(0, 1), 80);
            entity.Avatar = ImageEditor.imageToByteArray(img);

            //TODO: Kullanıcıya şifresi email gönderilecek.
            //var send = Tools.MailSender.SendEmail("gulsahs@yahoo.com", "DENEME", "Test maili denemesi");

            db.Users.Add(entity);
            await db.SaveChangesAsync();

            var currentUser = db.Users.FirstOrDefault(x => x.Email == user.Email);
            if (currentUser != null)
            {
                if (currentUser.IsAdmin) { AddMenuAccess(currentUser.Id, "Admin"); }
                else if (currentUser.IsSuperUser) { AddMenuAccess(currentUser.Id, "SuperUser"); }
                else { AddMenuAccess(currentUser.Id, "Everyone"); }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await db.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Languages = new SelectList(db.Languages, "Id", "LongName");
            ViewBag.Token = Security.GetToken(user.Id.ToString());

            return View(user);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [FromForm] User user, IFormFile file)
        {
            if (id != user.Id) return NotFound();
            if (!ModelState.IsValid) return View(user);

            var entity = db.Users.Find(id);
            var userIsAdmin = entity.IsAdmin;
            var userIsSuperUser = entity.IsSuperUser;

            entity.FirstName = user.FirstName;
            entity.LastName = user.LastName;
            entity.IsAdmin = user.IsAdmin;
            entity.IsSuperUser = user.IsSuperUser;
            entity.Status = user.Status;
            entity.Email = user.Email;
            entity.Phone = user.Phone;
            entity.LanguageId = user.LanguageId;

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
                    img = ImageEditor.SquareToLongEdge(img);
                    entity.Avatar = ImageEditor.imageToByteArray(img);
                }
            }


            if (userIsAdmin != user.IsAdmin || userIsSuperUser != user.IsSuperUser)
            {
                RemoveAllMenuAccess(entity.Id);

                if (user.IsAdmin) { AddMenuAccess(entity.Id, "Admin"); }
                else if (user.IsSuperUser) { AddMenuAccess(entity.Id, "SuperUser"); }
                else { AddMenuAccess(entity.Id, "Everyone"); }
            }

            TempData["Message"] = "msgSaved";

            db.Update(entity);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Edit));
        }

        [LogFilter]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #region AccessRights

        public PartialViewResult _AccessRights(Guid id)
        {
            var mainMenu = db.Menu.Where(x => x.AdminMenu == false).ToList();
            var adminMenu = db.Menu.Where(x => x.AdminMenu == true).ToList();
            var userMenu = db.Menu_User.Where(x => x.UserId == id).ToList();

            var model = new AccessRightsViewModel()
            {
                UserId = id,
                MainMenu = mainMenu,
                AdminMenu = adminMenu,
                UserMenu = userMenu
            };

            return PartialView(model);
        }


        private async void RemoveAllMenuAccess(Guid id)
        {
            var userMenu = await db.Menu_User.Where(x => x.UserId == id).ToListAsync();
            db.Menu_User.RemoveRange(userMenu);
            await db.SaveChangesAsync();
        }

        private async void AddMenuAccess(Guid id, string access)
        {
            var menu = await db.Menu.ToListAsync();

            if (access == "SuperUser") menu = menu.Where(x => x.SuperUser == true).ToList();
            if (access == "Everyone") menu = menu.Where(x => x.Everyone == true).ToList();

            foreach (var item in menu)
            {
                db.Menu_User.Add(new Menu_User() { MenuId = item.Id, UserId = id, Create = true, Read = true, Update = true, Delete = true, List = true });
            }
            await db.SaveChangesAsync();
        }

        [LogFilter]
        public async Task<IActionResult> _ChangeAccessRight(Guid userId, int menuId, string accessType, bool accessRight)
        {
            var menu_user = await db.Menu_User.FirstOrDefaultAsync(x => x.UserId == userId && x.MenuId == menuId);
            if (menu_user == null)
            {
                menu_user = new Menu_User() { UserId = userId, MenuId = menuId, Create = false, Read = false, Update = false, Delete = false, List = false };
                db.Menu_User.Add(menu_user);
                await db.SaveChangesAsync();
            }

            var entity = await db.Menu_User.FirstOrDefaultAsync(x => x.UserId == userId && x.MenuId == menuId);

            if (accessType == "Create") entity.Create = accessRight;
            if (accessType == "Read") entity.Read = accessRight;
            if (accessType == "Update") entity.Update = accessRight;
            if (accessType == "Delete") entity.Delete = accessRight;
            if (accessType == "List") entity.List = accessRight;

            db.Menu_User.Update(entity);
            await db.SaveChangesAsync();

            return Ok();
        }

        #endregion

        #region Files

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(Guid id, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);

                    var entity = new Data.Entities.System.File()
                    {
                        FileName = file.FileName,
                        MIMEType = file.ContentType,
                        FileContent = ms.ToArray(),
                        RelatedTable = "User",
                        RelatedGuid = id, 
                        CreationDate = DateTime.UtcNow, 
                        CreatedBy = User.Id()
                    };

                    db.Files.Add(entity);
                    await db.SaveChangesAsync();
            
                } 
            }

            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                var model = db.Files.Where(x => x.RelatedTable == "User" && x.RelatedGuid == id).ToList();
                return PartialView("_Files", model);            
            }

            var url = Url.Action(nameof(Edit), new { id = id }) + "#files";
            return Redirect(url);
        }

        public async Task<IActionResult> DeleteFile(Guid userId, Guid fileId)
        {
            var file = await db.Files.FindAsync(fileId);
            db.Files.Remove(file);
            await db.SaveChangesAsync();

            var url = Url.Action(nameof(Edit), new { id = userId }) + "#files";
            return Redirect(url);
        }

        public PartialViewResult _Files(Guid id)
        {
            var model = db.Files.Where(x => x.RelatedTable == "User" && x.RelatedGuid == id).ToList();

            // VIEWMODEL ile kullanmak için
            //var model = db.Files.Where(x => x.RelatedTable == "User" && x.RelatedGuid.ToString() == id.ToString())
            //    .Select(x => new FileViewModel { Id = x.Id, FileName = x.FileName, FileType = x.FileType, FileGroupId = x.FileGroupId, MIMEType = x.MIMEType, CreationDate = x.CreationDate, CreatedBy = x.CreatedBy })
            //    .ToList();

            return PartialView(model);
        }

        public IActionResult File(Guid id)
        {
            var file = db.Files.Find(id);
            if (file != null)
            {
                return File(file.FileContent, file.MIMEType, file.FileName);
            }
            return NotFound();
        }

        #endregion

    }
}