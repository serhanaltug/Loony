using Loony.Data.Entities.System;
using Loony.Web.Extensions;
using Loony.Web.Filters;
using Loony.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Loony.Web.Controllers
{
    public class MenuController : BaseController
    {

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult _List(string search, string filters = "%5B%22AdminMenu:false%22%5D", string order = "Order", string sortdir = "asc", int page = 1)
        {
            var model = new ListViewModel<Menu>(filters, search, order, sortdir);

            IQueryable<Menu> menus = db.Menu;

            if (search != null && search.Length > 0)
            {
                menus = menus.Where(x =>
                x.Name.ToLower().Contains(search.ToLower())
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

                menus = menus.Where(filter);
            }


            if (order != null)
            {
                menus = menus.OrderBy(order, sortdir.Contains("desc"));
            }

            model.Page.DataCount = menus.Count();
            model.Page.SelectedPage = page;
            model.Data = PaginatedList<Menu>.CreateAsync(menus.AsNoTracking(), page, model.Page.PageSize).Result;

            var path = Path.Combine(env.ContentRootPath, "wwwroot", "lib", "FlatIcons.csv");
            var file = System.IO.File.ReadAllLines(path);
            ViewBag.FlatIcons = file;

            return PartialView(model);

        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([FromForm] Menu model)
        {
            if (model == null || !ModelState.IsValid) return RedirectToAction(nameof(Index));

            var entity = new Menu();

            var nextId = db.Menu.Max(x => x.Id) + 1;
            entity.Id = nextId;

            var nextOrder = db.Menu.Where(x => x.AdminMenu == model.AdminMenu).Max(x => x.Order) + 1;
            entity.Order = nextOrder;

            entity.Name = model.Name;
            entity.Controller = model.Controller;
            entity.Action = model.Action;
            entity.Icon = model.Icon;
            entity.RelatedId = model.RelatedId;
            entity.LinkId = model.LinkId;
            entity.Multi = model.Multi;
            entity.SuperUser = model.SuperUser;
            entity.Everyone = model.Everyone;
            entity.AdminMenu = model.AdminMenu;


            db.Menu.Add(entity);
            await db.SaveChangesAsync();

            var adminUsers = db.Users.Where(x => x.IsAdmin == true).ToList();
            foreach (var item in adminUsers)
            {
                db.Menu_User.Add(new Menu_User() { MenuId = entity.Id, UserId = item.Id, Create = true, Read = true, Update = true, Delete = true, List = true });
            }


            if (model.SuperUser == true)
            {
                var superUsers = db.Users.Where(x => x.IsSuperUser == true).ToList();
                foreach (var item in superUsers)
                {
                    var userMenu = db.Menu_User.FirstOrDefault(x => x.MenuId == entity.Id && x.UserId == item.Id);
                    if (userMenu == null)
                    {
                        db.Menu_User.Add(new Menu_User() { MenuId = entity.Id, UserId = item.Id, Create = true, Read = true, Update = true, Delete = true, List = true });
                    }
                }
                await db.SaveChangesAsync();
            }

            if (model.Everyone == true)
            {
                foreach (var item in db.Users.ToList())
                {
                    var userMenu = db.Menu_User.FirstOrDefault(x => x.MenuId == entity.Id && x.UserId == item.Id);
                    if (userMenu == null)
                    {
                        db.Menu_User.Add(new Menu_User() { MenuId = entity.Id, UserId = item.Id, Create = true, Read = true, Update = true, List = true });
                    }
                }
                await db.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> _Edit(int id)
        {
            var entity = await db.Menu.FindAsync(id);
            if (entity == null) return BadRequest();

            var path = Path.Combine(env.ContentRootPath, "wwwroot", "lib", "FlatIcons.csv");
            var file = System.IO.File.ReadAllLines(path);
            ViewBag.FlatIcons = file;

            return PartialView(entity);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _Edit([FromForm] Menu model)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();

            var entity = await db.Menu.FindAsync(model.Id);
            if (entity == null) return BadRequest();

            var OrderChange = false;

            if (model.Order != entity.Order)
            {
                var actualOrder = entity.Order;
                var maxOrder = db.Menu.Where(x => x.AdminMenu == entity.AdminMenu).Max(x => x.Order);
                if (model.Order < 1) { model.Order = 1; }

                if (model.Order > maxOrder) { model.Order = maxOrder; }

                if (model.Order < actualOrder)
                {
                    for (int i = actualOrder - 1; i >= model.Order; i--)
                    {
                        var o = db.Menu.Where(x => x.AdminMenu == entity.AdminMenu).FirstOrDefault(x => x.Order == i);
                        o.Order = i + 1;
                        db.Update(o);
                        db.SaveChanges();
                    }
                }
                else
                {
                    for (int i = entity.Order + 1; i <= model.Order; i++)
                    {
                        var o = db.Menu.Where(x => x.AdminMenu == entity.AdminMenu).FirstOrDefault(x => x.Order == i);
                        o.Order = i - 1;
                        db.Update(o);
                        db.SaveChanges();
                    }
                }
                entity.Order = model.Order;
                OrderChange = true;
            }

            if (model.SuperUser != entity.SuperUser)
            {
                var superUsers = db.Users.Where(x => x.IsSuperUser == true).ToList();
                if (model.SuperUser == true)
                {
                    foreach (var item in superUsers)
                    {
                        var userMenu = db.Menu_User.FirstOrDefault(x => x.MenuId == model.Id && x.UserId == item.Id);
                        if (userMenu == null)
                        {
                            db.Menu_User.Add(new Menu_User() { MenuId = model.Id, UserId = item.Id, Create = true, Read = true, Update = true, Delete = true, List = true });
                        }
                    }
                }
                else
                {
                    foreach (var item in superUsers)
                    {
                        var userMenu = db.Menu_User.FirstOrDefault(x => x.MenuId == model.Id && x.UserId == item.Id);
                        if (userMenu != null)
                        {
                            db.Menu_User.Remove(userMenu);
                        }
                    }
                }
                db.SaveChanges();
                entity.SuperUser = model.SuperUser;
            }

            if (model.Everyone != entity.Everyone)
            {
                if (model.Everyone == true)
                {
                    foreach (var item in db.Users.ToList())
                    {
                        var userMenu = db.Menu_User.FirstOrDefault(x => x.MenuId == model.Id && x.UserId == item.Id);
                        if (userMenu == null)
                        {
                            db.Menu_User.Add(new Menu_User() { MenuId = model.Id, UserId = item.Id, Create = true, Read = true, Update = true, List = true });
                        }
                    }
                }
                else
                {
                    var users = db.Users.Where(x => x.IsAdmin == false);
                    if (model.SuperUser == true) users = users.Where(x => x.IsSuperUser == false);
                    foreach (var item in users.ToList())
                    {
                        var userMenu = db.Menu_User.FirstOrDefault(x => x.MenuId == model.Id && x.UserId == item.Id);
                        if (userMenu != null)
                        {
                            db.Menu_User.Remove(userMenu);
                        }
                    }
                }
                db.SaveChanges();
                entity.Everyone = model.Everyone;
            }

            entity.Name = model.Name;
            entity.Controller = model.Controller;
            entity.Action = model.Action;
            entity.Icon = model.Icon;
            entity.RelatedId = model.RelatedId;
            entity.LinkId = model.LinkId;
            entity.Multi = model.Multi;

            db.Update(entity);
            await db.SaveChangesAsync();
            if (OrderChange)
                return Json(new { ok = true, filters = "%5B%22AdminMenu:" + entity.AdminMenu.ToString().ToLower() + "%22%5D" });
            return PartialView("_Row", entity);

        }

        [LogFilter]
        public async Task<IActionResult> _Delete(int id)
        {
            var count = db.Menu.Count();
            var entity = await db.Menu.FindAsync(id);
            if (entity == null) return BadRequest();

            db.Remove(entity);
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}