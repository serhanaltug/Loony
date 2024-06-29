using Loony.Data.Entities.Product;
using Loony.Tools;
using Loony.Web.Extensions;
using Loony.Web.Filters;
using Loony.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class CareInstructionController : BaseController
    {

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult _List(string filters, string search, string order = "Id", string sortdir = "asc", int page = 1)
        {
            var model = new ListViewModel<CareInstruction>(filters, search, order, sortdir);

            IQueryable<CareInstruction> careinstructions = db.CareInstructions;

            if (search != null && search.Length > 0)
            {
                careinstructions = careinstructions.Where(x =>
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

                careinstructions = careinstructions.Where(filter);
            }


            if (order != null)
            {
                careinstructions = careinstructions.OrderBy(order, sortdir.Contains("desc"));
            }

            model.Page.DataCount = careinstructions.Count();
            model.Page.SelectedPage = page;
            model.Data = PaginatedList<CareInstruction>.CreateAsync(careinstructions.AsNoTracking(), page, model.Page.PageSize).Result;

            return PartialView(model);

        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([FromForm] CareInstruction model, IFormFile file)
        {
            if (model == null || !ModelState.IsValid) return RedirectToAction(nameof(Index));

            if (file != null && file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    var img = Image.FromStream(ms);

                    model.Image = ImageEditor.imageToByteArray(img);
                }
            }

            db.CareInstructions.Add(model);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> _Edit(int id)
        {
            var entity = await db.CareInstructions.FindAsync(id);
            if (entity == null) return BadRequest();

            return PartialView(entity);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _Edit([FromForm] CareInstruction model, IFormFile file)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();

            var entity = await db.CareInstructions.FindAsync(model.Id);
            if (entity == null) return BadRequest();

            entity.Name = model.Name;
            entity.Group = model.Group;
            entity.Description = model.Description;

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

            return PartialView("_Row", entity);
        }

        [LogFilter]
        public async Task<IActionResult> _Delete(int id)
        {
            var entity = await db.CareInstructions.FindAsync(id);
            if (entity == null) return BadRequest();

            db.Remove(entity);
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}