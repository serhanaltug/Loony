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
    public class ProductController : BaseController
    {

        public IActionResult Index()
        {
            ViewBag.ProductGroups = new SelectList(db.ProductGroups.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GroupName }), "Value", "Text");
            return View();
        }

        public PartialViewResult _List(string filters, string search, string order = "Id", string sortdir = "desc", int page = 1, string type = "_List")
        {
            var model = new ListViewModel<Product>(filters, search, order, sortdir);

            IQueryable<Product> products = db.Products;
            products = products.Include(x => x.ProductGroup);
            products = products.Include(x => x.ProductImage);

            if (search != null && search.Length > 0)
            {
                products = products.Where(x =>
                x.ProductName.ToLower().Contains(search.ToLower()) ||
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

                products = products.Where(filter);
            }

            if (order != null)
            {
                products = products.OrderBy(order, sortdir.Contains("desc"));
            }

            model.Page.DataCount = products.Count();
            model.Page.SelectedPage = page;
            model.Data = PaginatedList<Product>.CreateAsync(products.AsNoTracking(), page, model.Page.PageSize).Result;

            return PartialView(type, model);
        }


        public IActionResult New()
        {
            //var nextProductCode = 1;
            //nextProductCode = db.Products.Max(x => x.ProductCode) + 1;

            //if (db.Products.Any(x => x.Year == DateTime.Today.Year+1))
            //nextProductCode = db.Products.Where(x => x.Year == DateTime.Today.Year+1).Max(x => x.ProductCode) + 1;

            var model = new ProductViewModel()
            {
                Product = new Product
                {
                    //ProductCode = nextProductCode,
                    Season = "Summer",
                    Year = DateTime.Today.Year + 1,

                },
                ProductGroupSelectList = new SelectList(db.ProductGroups.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GroupName }), "Value", "Text", null)
            };

            return View(model);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([FromForm] ProductViewModel model, IFormFile file)
        {
            if (!ModelState.IsValid) return View(model);

            var entity = model.Product;

            entity.Id = new Guid();
            entity.CreationDate = DateTime.Now;
            entity.CreatedBy = User.Id();
            entity.ModificationDate = DateTime.Now;
            entity.ModifiedBy = User.Id();


            db.Products.Add(entity);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var entity = await db.Products
                //.Include(pma => pma.ProductChain).ThenInclude(m => m.Model)
                //.Include(pma => pma.ProductChain).ThenInclude(a => a.Article)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null) return NotFound();
            var productImages = db.ProductImages.Where(x => x.ProductId == id).OrderBy(x => x.Order).Select(x => x.Id).ToList();
            var productBaseImage = db.ProductImages.FirstOrDefault(x => x.ProductId == id && x.Base == true);

            var model = new ProductViewModel()
            {
                Id = id,
                Product = entity,
                Images = productImages,
                BaseImageId = (productBaseImage != null) ? productBaseImage.Id : 0,
                ProductGroupSelectList = new SelectList(db.ProductGroups.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.GroupName }), "Value", "Text", entity.ProductGroupId)
            };

            return View(model);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] ProductViewModel model, IFormFile file)
        {
            if (!ModelState.IsValid) return View(model);

            model.Product.ModificationDate = DateTime.Now;
            model.Product.ModifiedBy = User.Id();

            var entity = model.Product;

            //db.Product_Model.RemoveRange(db.Product_Model.Where(x => x.ProductId == model.Id));

            //foreach (var item in model.Models) {
            //    db.Product_Model.Add(new Product_Model() { ModelId = Convert.ToInt32(item.Value), ProductId = model.Id });
            //}


            TempData["Message"] = "msgSaved";

            db.Update(entity);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Edit));
        }

        [LogFilter]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        #region ProductChain

        public PartialViewResult _ProductChain(Guid id)
        {
            var model = db.ProductChains.Where(x => x.ProductId == id)
                .Include(x => x.Model)
                .Include(x => x.Article)
                .Include(x => x.Accessory)
                .ToList();

            return PartialView(model);


        }

        #endregion

        #region ProductImages

        public PartialViewResult _Image(Guid id, int? imageid)
        {
            var maxImageOrder = 0;
            if (db.ProductImages.Any(x => x.ProductId == id))
                maxImageOrder = db.ProductImages.Where(x => x.ProductId == id).Max(x => x.Order);
            var entity = new ProductImageViewModel()
            {
                ProductImage = new ProductImage() { ProductId = id, Order = maxImageOrder + 1, Base = (maxImageOrder == 0) }
            };

            if (imageid != null)
                entity.ProductImage = db.ProductImages.FirstOrDefault(x => x.Id == imageid);

            return PartialView(entity);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _Image(ProductImageViewModel model, IFormFile file)
        {
            ModelState.Clear();

            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var TempImage = Image.FromStream(ms);

                    int OrginalWidth, OrginalHeight;

                    int TempHeight = TempImage.Height;
                    int TempWidth = TempImage.Width;

                    if (TempWidth > 900)
                        OrginalWidth = 900;
                    else
                        OrginalWidth = TempWidth;

                    OrginalHeight = (int)(OrginalWidth * TempHeight / (double)TempWidth);

                    TempImage.Dispose();

                    var img = ImageEditor.FixedSize(Image.FromStream(ms), OrginalWidth, OrginalHeight);
                    model.ProductImage.Image = ImageEditor.imageToByteArray(img);

                }
            }

            var maxOrder = db.ProductImages.Where(x => x.ProductId == model.ProductImage.ProductId).Count();

            if (model.ProductImage.Id != 0) // Kayıtlı resim editleniyorsa
            {
                var entity = db.ProductImages.Find(model.ProductImage.Id);
                entity.Base = model.ProductImage.Base;
                if (file != null && file.Length > 0)
                    entity.Image = model.ProductImage.Image;

                if (entity.Order != model.ProductImage.Order)
                {
                    if (model.ProductImage.Order < 1) { model.ProductImage.Order = 1; }
                    if (model.ProductImage.Order > maxOrder) { model.ProductImage.Order = maxOrder; }

                    ReorderImages(model.ProductImage.ProductId, model.ProductImage.Id, model.ProductImage.Order);
                    //var actualOrder = db.ProductImages.FirstOrDefault(x => x.ProductId == model.ProductImage.ProductId && x.Order == model.ProductImage.Order);
                    //actualOrder.Order = entity.Order;
                    //db.Update(actualOrder);

                    entity.Order = model.ProductImage.Order;
                }

                db.Update(entity);
            }
            else // İlk yada yeni kayıt
            {
                if (!db.ProductImages.Any(x => x.ProductId == model.ProductImage.ProductId))
                    model.ProductImage.Base = true;

                if (model.ProductImage.Order < 1) { model.ProductImage.Order = 1; }
                if (model.ProductImage.Order > maxOrder) { model.ProductImage.Order = maxOrder + 1; }

                for (int i = maxOrder; i >= model.ProductImage.Order; i--)
                {
                    var o = db.ProductImages.FirstOrDefault(x => x.ProductId == model.ProductImage.ProductId && x.Order == i);
                    o.Order = i + 1;
                    db.Update(o);
                    await db.SaveChangesAsync();
                }

                if (file == null || file.Length == 0)
                {
                    ModelState.AddModelError("imgError", "ProductImageRequired");
                    return PartialView(model);
                }
                db.Add(model.ProductImage);
            }

            await db.SaveChangesAsync();

            if (model.ProductImage.Base) // Diğer kayıtların base'ini kaldır.
            {
                await db.ProductImages.Where(x => x.ProductId == model.ProductImage.ProductId && x.Id != model.ProductImage.Id).ForEachAsync(x => x.Base = false);
                await db.SaveChangesAsync().ConfigureAwait(false);
            }

            return PartialView(model);
        }

        public async Task<IActionResult> _DeleteImage(Guid id, int imageid)
        {
            var imageCount = db.ProductImages.Where(x => x.ProductId == id).Count();
            var productImage = await db.ProductImages.FindAsync(imageid);

            if (imageCount > 1)
            {
                if (productImage.Base)
                {
                    var minOrder = db.ProductImages.Where(x => x.ProductId == id && x.Id != imageid).Min(x => x.Order);
                    var newBase = db.ProductImages.FirstOrDefault(x => x.ProductId == id && x.Order == minOrder);
                    newBase.Base = true;
                    db.ProductImages.Update(newBase);
                    await db.SaveChangesAsync();
                }

                for (int i = productImage.Order + 1; i <= imageCount; i++)
                {
                    var o = db.ProductImages.FirstOrDefault(x => x.ProductId == id && x.Order == i);
                    o.Order = i - 1;
                    db.Update(o);
                    await db.SaveChangesAsync();
                }

            }

            db.ProductImages.Remove(productImage);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Edit), new { Id = id });
        }

        private async void ReorderImages(Guid ProductId, int ImageId, int modelOrder)
        {
            var maxOrder = 0;
            if (db.ProductImages.Any(x => x.ProductId == ProductId))
                maxOrder = db.ProductImages.Where(x => x.ProductId == ProductId).Max(x => x.Order);

            var actualOrder = db.ProductImages.FirstOrDefault(x => x.Id == ImageId).Order;

            if (modelOrder < 1) { modelOrder = 1; }

            if (modelOrder > maxOrder) { modelOrder = maxOrder; }

            if (modelOrder < actualOrder)
            {
                for (int i = actualOrder - 1; i >= modelOrder; i--)
                {
                    var o = db.ProductImages.FirstOrDefault(x => x.ProductId == ProductId && x.Order == i);
                    o.Order = i + 1;
                    db.Update(o);
                    await db.SaveChangesAsync();
                }
            }
            else
            {
                for (int i = actualOrder + 1; i <= modelOrder; i++)
                {
                    var o = db.ProductImages.FirstOrDefault(x => x.ProductId == ProductId && x.Order == i);
                    o.Order = i - 1;
                    db.Update(o);
                    await db.SaveChangesAsync();
                }
            }
        }

        #endregion
    }
}