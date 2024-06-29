using Loony.Data.Entities.System;
using Loony.Web.Extensions;
using Loony.Web.Filters;
using Loony.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Web;

namespace Loony.Web.Controllers
{
    public class TranslationController : BaseController
    {

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult _List(string filters, string search, string order = "Id", string sortdir = "desc", int page = 1)
        {
            var model = new ListViewModel<Translation>(filters, search, order, sortdir);

            IQueryable<Translation> translations = db.Translations;

            if (search != null && search.Length > 0)
            {
                translations = translations.Where(x =>
                x.Key.ToLower().Contains(search.ToLower()) ||
                x.EN.ToLower().Contains(search.ToLower()) ||
                x.TR.ToLower().Contains(search.ToLower())
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

                translations = translations.Where(filter);
            }

            if (order != null)
            {
                translations = translations.OrderBy(order, sortdir.Contains("desc"));
            }

            model.Page.DataCount = translations.Count();
            model.Page.SelectedPage = page;
            model.Data = PaginatedList<Translation>.CreateAsync(translations.AsNoTracking(), page, model.Page.PageSize).Result;

            return PartialView(model);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> New([FromForm] Translation model)
        {
            if (model == null || !ModelState.IsValid) return RedirectToAction(nameof(Index));

            db.Translations.Add(model);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> _Edit(int id)
        {
            var entity = await db.Translations.FindAsync(id);
            if (entity == null) return BadRequest();

            return PartialView(entity);
        }

        [LogFilter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _Edit([FromForm] Translation model)
        {
            if (model == null || !ModelState.IsValid) return BadRequest();

            var entity = await db.Translations.FindAsync(model.Id);
            if (entity == null) return BadRequest();

            entity.EN = model.EN;
            entity.TR = model.TR;
            entity.Group = model.Group;

            db.Update(entity);
            await db.SaveChangesAsync();

            return PartialView("_Row", entity);
        }

        [LogFilter]
        public async Task<IActionResult> _Delete(int id)
        {
            var entity = await db.Translations.FindAsync(id);
            if (entity == null) return BadRequest();

            db.Remove(entity);
            await db.SaveChangesAsync();

            return Ok();
        }

        public IActionResult Export()
        {

            var data = new StringBuilder();
            data.AppendLine("Id;Key;EN;TR;Type;Group");

            foreach (var item in db.Translations.ToList())
            {
                data.AppendLine($"{item.Id};{item.Key};{item.EN};{item.TR};{item.Type};{item.Group}");
            }

            var content = Encoding.UTF8.GetBytes(data.ToString());
            var result = Encoding.UTF8.GetPreamble().Concat(content).ToArray();

            return File(result, "text/csv", $"Translations_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}.csv");
        }

        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Import(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                string fileExtension = Path.GetExtension(file.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension == ".csv")
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        db.Translations.RemoveRange(db.Translations);

                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            var datarow = line.Split(";");

                            if (datarow[0] != null && datarow[0].IsNumeric() && Convert.ToInt32(datarow[0]) > 0)
                            {

                                var entity = new Translation();

                                entity.Id = Convert.ToInt32(datarow[0]);
                                entity.Key = datarow[1];
                                entity.EN = datarow[2];
                                entity.TR = datarow[3];
                                entity.Type = (datarow[4] != null) ? Convert.ToInt32(datarow[4]) : 0;
                                entity.Group = datarow[5];

                                db.Translations.Add(entity);

                            }
                        }
                    }

                    db.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }

            TempData["Message"] = "msgImportError";
            return View();
        }

        public IActionResult Translate(string key)
        {
            var result = string.Empty;
            var text = db.Translations.Where(x => x.Key == key).FirstOrDefault();
            if (text != null)
            {
                if (User.Language() == "TR")
                    return Content(text.TR);
                else
                    return Content(text.EN);
            }
            return Content($"[{key}]");

        }
    }
}
