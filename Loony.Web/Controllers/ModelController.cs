using Loony.Data.Entities.Product;
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
    public class ModelController : BaseController
    {

        public IActionResult Index()
        {
            ViewBag.ModelGroups = new SelectList(db.ProductGroups.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GroupName }), "Value", "Text");
            return View();
        }

        public PartialViewResult _List(string filters, string search, string order = "Id", string sortdir = "desc", int page = 1, string type = "_List")
        {
            var model = new ListViewModel<Model>(filters, search, order, sortdir);

            IQueryable<Model> models = db.Models;
            models = models.Include(x => x.ModelGroup);

            if (search != null && search.Length > 0)
            {
                models = models.Where(x =>
                x.ModelName.ToLower().Contains(search.ToLower()) ||
                x.Year.Equals(search) ||
                x.Season.ToLower().Contains(search.ToLower()) ||
                x.Description.ToLower().Contains(search.ToLower())
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

                models = models.Where(filter);
            }

            if (order != null)
            {
                models = models.OrderBy(order, sortdir.Contains("desc"));
            }

            model.Page.DataCount = models.Count();
            model.Page.SelectedPage = page;
            model.Data = PaginatedList<Model>.CreateAsync(models.AsNoTracking(), page, model.Page.PageSize).Result;
            return PartialView(type, model);
        }

        public IActionResult New()
        {
            var nextModelCode = 1;
            nextModelCode = db.Models.Max(x => x.ModelCode) + 1;

            var model = new ModelViewModel()
            {
                ProductModel = new Model
                {
                    ModelCode = nextModelCode,
                    Season = "SS - Spring/Summer",
                    Year = DateTime.Today.Year + 1,

                },
                ModelGroupSelectList = new SelectList(db.ProductGroups.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GroupName }), "Value", "Text", null)
            };

            return View(model);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([FromForm] ModelViewModel model, IFormFile file)
        {
            if (!ModelState.IsValid) return View(model);

            var entity = model.ProductModel;

            entity.Id = db.Models.Max(x => x.Id) + 1;
            entity.CreationDate = DateTime.Now;
            entity.CreatedBy = User.Id();
            entity.ModificationDate = DateTime.Now;
            entity.ModifiedBy = User.Id();

            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var img = Image.FromStream(ms);

                    entity.Image = ImageEditor.imageToByteArray(img);
                }
            }

            db.Models.Add(entity);
            await db.SaveChangesAsync();

            if (model.Tags != null && model.Tags.Count() > 0)
            {
                foreach (var item in model.Tags)
                {
                    if (item.IsNumeric())
                    {
                        db.Model_Tag.Add(new Model_Tag() { TagId = Convert.ToInt32(item), ModelId = entity.Id });
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        db.Tags.Add(new Tag() { TagName = item });
                        await db.SaveChangesAsync();
                        int newTagId = db.Tags.FirstOrDefault(x => x.TagName == item).Id;
                        db.Model_Tag.Add(new Model_Tag() { TagId = newTagId, ModelId = entity.Id });
                        await db.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await db.Models
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return NotFound();

            var model = new ModelViewModel()
            {
                Id = id,
                ProductModel = entity,
                TagSelectList = db.Model_Tag.Include(x => x.Tags).Where(t => t.ModelId == id).Select(x => new SelectListItem { Value = x.Tags.Id.ToString(), Text = x.Tags.TagName, Selected = true }).ToList(),
                ModelGroupSelectList = new SelectList(db.ProductGroups.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GroupName }), "Value", "Text", entity.ModelGroupId)
            };

            return View(model);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ModelViewModel model, IFormFile file)
        {
            if (!ModelState.IsValid) return View(model);

            var entity = db.Models.Find(model.ProductModel.Id);

            entity.ModelCode = model.ProductModel.ModelCode;
            entity.ModelName = model.ProductModel.ModelName;
            entity.Season = model.ProductModel.Season;
            entity.Year = model.ProductModel.Year;
            entity.Description = model.ProductModel.Description;
            entity.ModelGroupId = model.ProductModel.ModelGroupId;

            entity.ModificationDate = DateTime.Now;
            entity.ModifiedBy = User.Id();


            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var img = Image.FromStream(ms);

                    entity.Image = ImageEditor.imageToByteArray(img);
                }
            }

            TempData["Message"] = "msgSaved";

            db.Update(entity);
            await db.SaveChangesAsync();

            db.Model_Tag.RemoveRange(db.Model_Tag.Where(x => x.ModelId == entity.Id));
            await db.SaveChangesAsync();

            if (model.Tags != null && model.Tags.Count() > 0)
            {
                foreach (var item in model.Tags)
                {
                    if (item.IsNumeric())
                    {
                        db.Model_Tag.Add(new Model_Tag() { TagId = Convert.ToInt32(item), ModelId = entity.Id });
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        db.Tags.Add(new Tag() { TagName = item });
                        await db.SaveChangesAsync();
                        int newTagId = db.Tags.FirstOrDefault(x => x.TagName == item).Id;
                        db.Model_Tag.Add(new Model_Tag() { TagId = newTagId, ModelId = entity.Id });
                        await db.SaveChangesAsync();
                    }
                }
            }

            return RedirectToAction(nameof(Edit));
        }

        [LogFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await db.Models.FindAsync(id);
            db.Models.Remove(model);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Tags(string search)
        {
            if (!(string.IsNullOrEmpty(search) || string.IsNullOrWhiteSpace(search)))
            {
                var tags = await db.Tags
                    .Where(x => x.TagName.ToLower().StartsWith(search.ToLower()))
                    .Select(x => new { id = x.Id, text = x.TagName })
                    .ToListAsync();

                return Json(new { items = tags });
            }

            return BadRequest();
        }

    }
}