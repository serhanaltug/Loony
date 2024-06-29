using Loony.Data.Entities.Product;
using Loony.Web.Extensions;
using Loony.Web.Filters;
using Loony.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Web;

namespace Loony.Web.Controllers
{
    public class TagController : BaseController
    {

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult _List(string filters, string search, string order = "Id", string sortdir = "desc", int page = 1)
        {
            var model = new ListViewModel<Tag>(filters, search, order, sortdir);

            IQueryable<Tag> tags = db.Tags;

            if (search != null && search.Length > 0)
            {
                tags = tags.Where(x =>
                x.TagName.ToLower().Contains(search.ToLower())
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

                tags = tags.Where(filter);
            }


            if (order != null)
            {
                tags = tags.OrderBy(order, sortdir.Contains("desc"));
            }

            model.Page.DataCount = tags.Count();
            model.Page.SelectedPage = page;
            model.Data = PaginatedList<Tag>.CreateAsync(tags.AsNoTracking(), page, model.Page.PageSize).Result;

            return PartialView(model);

        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([FromForm] Tag model)
        {
            if (model == null || !ModelState.IsValid) return RedirectToAction(nameof(Index));

            db.Tags.Add(model);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> _Edit(int id)
        {
            var entity = await db.Tags.FindAsync(id);
            if (entity == null) return BadRequest();

            return PartialView(entity);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _Edit([FromForm] Tag model)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();

            var entity = await db.Tags.FindAsync(model.Id);
            if (entity == null) return BadRequest();

            entity.TagName = model.TagName;

            db.Update(entity);
            await db.SaveChangesAsync();

            return PartialView("_Row", entity);
        }

        [LogFilter]
        public async Task<IActionResult> _Delete(int id)
        {
            var entity = await db.Tags.FindAsync(id);
            if (entity == null) return BadRequest();

            db.Remove(entity);
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}