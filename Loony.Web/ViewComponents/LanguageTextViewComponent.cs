using Loony.Data;
using Loony.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Loony.Web.ViewComponents
{
    public class LanguageTextViewComponent : ViewComponent
    {
        private readonly IMemoryCache _memoryCache;
        private readonly DataContext db;

        public LanguageTextViewComponent(IMemoryCache memoryCache, DataContext dbContext)
        {
            _memoryCache = memoryCache;
            db = dbContext;
        }

        public string Invoke(string key, int type)
        {
            string text = "";
            var language = Request.HttpContext.User.Language() ?? "EN";

            if (_memoryCache.Get(language + "-" + key) == null)
            {
                var dbText = db.Translations
                    .Where(x => x.Key == key && (int)x.Type == type)
                    .Select(x => x.GetType().GetProperty(language).GetValue(x, null))
                    .FirstOrDefault();

                if (dbText == null)
                    text = "[" + key + "]";
                else
                    text = dbText.ToString();

                var cacheExpirationOptions = new MemoryCacheEntryOptions { AbsoluteExpiration = DateTime.Now.AddDays(1), Priority = CacheItemPriority.Normal };
                _memoryCache.Set(language + "-" + key, text, cacheExpirationOptions);
            }

            return _memoryCache.Get(language + "-" + key).ToString();
        }

    }
}
