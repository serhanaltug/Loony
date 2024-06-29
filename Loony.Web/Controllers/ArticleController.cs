using Loony.Data;
using Loony.Data.Entities.Product;
using Loony.Tools;
using Loony.Web.Extensions;
using Loony.Web.Filters;
using Loony.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    [Authorize, AuthorizationFilter, ExceptionFilter]
    public class ArticleController : Controller
    {
        private readonly DataContext db;
        private IWebHostEnvironment _env;

        public ArticleController(DataContext dbContext, IWebHostEnvironment env)
        {
            db = dbContext;
            _env = env;
        }

        public IActionResult Index()
        {
            ViewBag.DesignTypes = new SelectList(db.DesignTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.DesignTypeName }), "Value", "Text");
            return View();
        }

        public PartialViewResult _List(string filters, string search, string order = "Id", string sortdir = "desc", int page = 1, string type = "_List")
        {
            var model = new ListViewModel<Article>(filters, search, order, sortdir);

            IQueryable<Article> models = db.Articles;

            if (search != null && search.Length > 0)
            {
                models = models.Where(x =>
                x.ArticleName.ToLower().Contains(search.ToLower())
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
            model.Data = PaginatedList<Article>.CreateAsync(models.AsNoTracking(), page, model.Page.PageSize).Result;

            return PartialView(type, model);
        }

        public IActionResult New()
        {
            //var nextArticleCode = 1;
            var nextArticleCode = db.Articles.Max(x => x.ArticleCode) + 1;


            var model = new ArticleViewModel()
            {
                Article = new Article
                {
                    ArticleCode = nextArticleCode
                }
            };

            return View(model);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([FromForm] ArticleViewModel model, IFormFile file)
        {
            if (!ModelState.IsValid) return View(model);

            var entity = model.Article;

            entity.Id = db.Articles.Max(x => x.Id) + 1;
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

            db.Articles.Add(entity);
            await db.SaveChangesAsync();

            if (model.DesignTypes != null && model.DesignTypes.Count() > 0)
            {
                foreach (var item in model.DesignTypes)
                {
                    if (item.IsNumeric())
                    {
                        db.Article_DesignType.Add(new Article_DesignType() { DesignTypeId = Convert.ToInt32(item), ArticleId = entity.Id });
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        db.DesignTypes.Add(new DesignType() { DesignTypeName = item });
                        await db.SaveChangesAsync();
                        int newDesignTypeId = db.DesignTypes.FirstOrDefault(x => x.DesignTypeName == item).Id;
                        db.Article_DesignType.Add(new Article_DesignType() { DesignTypeId = newDesignTypeId, ArticleId = entity.Id });
                        await db.SaveChangesAsync();
                    }
                }
            }


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await db.Articles
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return NotFound();

            var model = new ArticleViewModel()
            {
                Id = id,
                Article = entity,
                DesignTypeSelectList = db.Article_DesignType.Include(x => x.DesignTypes).Where(t => t.ArticleId == id).Select(x => new SelectListItem { Value = x.DesignTypes.Id.ToString(), Text = x.DesignTypes.DesignTypeName, Selected = true }).ToList(),
            };

            return View(model);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ArticleViewModel model, IFormFile file)
        {

            if (!ModelState.IsValid) return View(model);

            var entity = db.Articles.Find(model.Article.Id);

            entity.ArticleCode = model.Article.ArticleCode;
            entity.ArticleName = model.Article.ArticleName;
            entity.ColorCode = model.Article.ColorCode;
            entity.DesignCode = model.Article.DesignCode;
            entity.Composition = model.Article.Composition;
            entity.Description = model.Article.Description;
            entity.Supplier = model.Article.Supplier;

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



            db.Update(entity);
            await db.SaveChangesAsync();

            db.Article_DesignType.RemoveRange(db.Article_DesignType.Where(x => x.ArticleId == entity.Id));
            await db.SaveChangesAsync();

            if (model.DesignTypes != null && model.DesignTypes.Count() > 0)
            {
                foreach (var item in model.DesignTypes)
                {
                    if (item.IsNumeric())
                    {
                        db.Article_DesignType.Add(new Article_DesignType() { DesignTypeId = Convert.ToInt32(item), ArticleId = entity.Id });
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        db.DesignTypes.Add(new DesignType() { DesignTypeName = item });
                        await db.SaveChangesAsync();
                        int newDesignTypeId = db.DesignTypes.FirstOrDefault(x => x.DesignTypeName == item).Id;
                        db.Article_DesignType.Add(new Article_DesignType() { DesignTypeId = newDesignTypeId, ArticleId = entity.Id });
                        await db.SaveChangesAsync();
                    }
                }
            }

            TempData["Message"] = "msgSaved";
            return RedirectToAction(nameof(Edit));
        }

        [LogFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await db.Articles.FindAsync(id);
            db.Articles.Remove(model);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DesignTypes(string search)
        {
            if (!(string.IsNullOrEmpty(search) || string.IsNullOrWhiteSpace(search)))
            {
                var designtypes = await db.DesignTypes
                    .Where(x => x.DesignTypeName.ToLower().Contains(search.ToLower()))
                    .Select(x => new { id = x.Id, text = x.DesignTypeName })
                    .ToListAsync();

                return Json(new { items = designtypes });
            }

            return BadRequest();
        }


    }
}