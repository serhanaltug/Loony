using Loony.Data.Entities.Product;
using Loony.Tools;
using Loony.Web.Extensions;
using Loony.Web.Filters;
using Loony.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Text.Json;
using System.Web;

namespace Loony.Web.Controllers
{
    public class AccessoryController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.AccessoryTypes = new SelectList(db.AccessoryTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.AccessoryTypeName }).OrderBy(x => x.Text), "Value", "Text");
            return View();
        }

        public PartialViewResult _List(string filters, string search, string order = "Id", string sortdir = "desc", int page = 1, string type = "_List")
        {
            var model = new ListViewModel<Accessory>(filters, search, order, sortdir);

            IQueryable<Accessory> models = db.Accessories;
            models = models.Include(x => x.AccessoryType);

            if (search != null && search.Length > 0)
            {
                models = models.Where(x =>
                x.AccessoryName.ToLower().Contains(search.ToLower())
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
            model.Data = PaginatedList<Accessory>.CreateAsync(models.AsNoTracking(), page, model.Page.PageSize).Result;

            return PartialView(type, model);
        }

        public IActionResult New()
        {
            //var nextAccessoryCode = 1;
            var nextAccessoryCode = db.Accessories.Max(x => x.AccessoryCode) + 1;

            var model = new AccessoryViewModel()
            {
                Accessory = new Accessory
                {
                    AccessoryCode = nextAccessoryCode
                },

                AccessoryTypeSelectList = new SelectList(db.AccessoryTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.AccessoryTypeName }).OrderBy(x => x.Text), "Value", "Text", null)

            };



            return View(model);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([FromForm] AccessoryViewModel model, IFormFile file)
        {
            if (!ModelState.IsValid) return View(model);

            var entity = model.Accessory;

            entity.Id = db.Accessories.Max(x => x.Id) + 1;
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

            db.Accessories.Add(entity);
            await db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var entity = await db.Accessories
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return NotFound();

            var model = new AccessoryViewModel()
            {
                Id = id,
                Accessory = entity,
                AccessoryTypeSelectList = new SelectList(db.AccessoryTypes.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.AccessoryTypeName }).OrderBy(x => x.Text), "Value", "Text", entity.AccessoryTypeId)
            };

            return View(model);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] AccessoryViewModel model, IFormFile file)
        {

            if (!ModelState.IsValid) return View(model);

            var entity = db.Accessories.Find(model.Accessory.Id);

            entity.AccessoryCode = model.Accessory.AccessoryCode;
            entity.AccessoryName = model.Accessory.AccessoryName;
            entity.AccessoryTypeId = model.Accessory.AccessoryTypeId;
            entity.Description = model.Accessory.Description;
            entity.Supplier = model.Accessory.Supplier;

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

            TempData["Message"] = "msgSaved";
            return RedirectToAction(nameof(Edit));
        }

        [LogFilter]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await db.Accessories.FindAsync(id);
            db.Accessories.Remove(model);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}