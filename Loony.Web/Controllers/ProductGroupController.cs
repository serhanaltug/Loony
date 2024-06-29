using Loony.Data.Entities.Product;
using Loony.Web.Extensions;
using Loony.Web.Filters;
using Loony.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Loony.Web.Controllers
{
    public class ProductGroupController : BaseController
    {

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult _List(string filters, string search, string order = "Order", string sortdir = "asc", int page = 1)
        {
            var model = new ListViewModel<ProductGroup>(filters, search, order, sortdir);

            IQueryable<ProductGroup> productgroups = db.ProductGroups;

            if (search != null && search.Length > 0)
            {
                productgroups = productgroups.Where(x =>
                x.GroupName.ToLower().Contains(search.ToLower())
                );
            }

            if (order != null)
            {
                productgroups = productgroups.OrderBy(order, sortdir.Contains("desc"));
            }

            model.Page.DataCount = productgroups.Count();
            model.Page.SelectedPage = page;
            model.Data = PaginatedList<ProductGroup>.CreateAsync(productgroups.AsNoTracking(), page, model.Page.PageSize).Result;

            return PartialView(model);

        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([FromForm] ProductGroup model)
        {
            if (model == null || !ModelState.IsValid) return RedirectToAction(nameof(Index));

            var count = db.ProductGroups.Count();
            for (int i = count; i >= model.Order; i--)
            {
                var o = db.ProductGroups.FirstOrDefault(x => x.Order == i);
                o.Order = i + 1;
                db.Update(o);
                await db.SaveChangesAsync();
            }

            db.ProductGroups.Add(model);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> _Edit(int id)
        {
            var entity = await db.ProductGroups.FindAsync(id);
            if (entity == null) return BadRequest();

            return PartialView(entity);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _Edit([FromForm] ProductGroup model)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();

            var entity = await db.ProductGroups.FindAsync(model.Id);
            if (entity == null) return BadRequest();
            var OrderChange = false;

            if (model.Order != entity.Order)
            {
                var actualOrder = entity.Order;
                var maxOrder = db.ProductGroups.Max(x => x.Order);
                if (model.Order < 1) { model.Order = 1; }

                if (model.Order > maxOrder) { model.Order = maxOrder; }

                if (model.Order < actualOrder)
                {
                    for (int i = actualOrder - 1; i >= model.Order; i--)
                    {
                        var o = db.ProductGroups.FirstOrDefault(x => x.Order == i);
                        o.Order = i + 1;
                        db.Update(o);
                        db.SaveChanges();
                    }
                }
                else
                {
                    for (int i = entity.Order + 1; i <= model.Order; i++)
                    {
                        var o = db.ProductGroups.FirstOrDefault(x => x.Order == i);
                        o.Order = i - 1;
                        db.Update(o);
                        db.SaveChanges();
                    }
                }
                entity.Order = model.Order;
                OrderChange = true;
            }

            entity.GroupName = model.GroupName;
            entity.Description = model.Description;

            db.Update(entity);
            await db.SaveChangesAsync();
            if (OrderChange)
                return Json(new { ok = true });
            //return Json(new { ok = true, url = Url.Action("Index") });
            return PartialView("_Row", entity);
        }

        [LogFilter]
        public async Task<IActionResult> _Delete(int id)
        {
            var count = db.ProductGroups.Count();
            var entity = await db.ProductGroups.FindAsync(id);
            if (entity == null) return BadRequest();

            for (int i = entity.Order + 1; i <= count; i++)
            {
                var o = db.ProductGroups.FirstOrDefault(x => x.Order == i);
                o.Order = i - 1;
                db.Update(o);
                await db.SaveChangesAsync();
            }

            db.Remove(entity);
            await db.SaveChangesAsync();

            return Json(new { ok = true });
        }
    }
}