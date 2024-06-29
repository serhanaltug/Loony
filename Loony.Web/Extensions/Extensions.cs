using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Principal;

namespace Loony.Web.Extensions
{
    public static class Extensions
    {

        #region Claims
        public static Guid Id(this IIdentity identity)
        {
            return new Guid((identity as ClaimsIdentity).FindFirst(ClaimTypes.NameIdentifier).ToString());
        }

        public static string GetClaim(this ClaimsPrincipal principal, string claim)
        {
            if (principal == null)
            {
                return "";
            }
            return principal.Claims.FirstOrDefault(x => x.Type == claim).Value;
        }

        public static Guid Id(this ClaimsPrincipal principal)
        {
            return new Guid(principal.Claims.FirstOrDefault(x => x.Type == "Id").Value);
        }

        public static string Email(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Email);
        }

        public static string Language(this ClaimsPrincipal principal)
        {
            return principal.Claims.FirstOrDefault(x => x.Type == "Language").Value;
        }

        public static string FullName(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Name);
        }

        public static bool IsAdmin(this ClaimsPrincipal principal)
        {
            var admin = principal.Claims.FirstOrDefault(x => x.Type == "IsAdmin").Value;

            if (admin == "True") return true;
            return false;
        }

        public static bool IsSuperUser(this ClaimsPrincipal principal)
        {
            var superuser = principal.Claims.FirstOrDefault(x => x.Type == "IsSuperUser").Value;

            if (superuser == "True") return true;
            return false;
        }
        #endregion

        public static IOrderedQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderByProperty, bool desc)
        {
            string command = desc ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },
                source.Expression, Expression.Quote(orderByExpression));
            return (IOrderedQueryable<TEntity>)source.Provider.CreateQuery<TEntity>(resultExpression);
        }


        public static IQueryable<T> Where<T>(this IQueryable<T> source, IList<Filter> filters)
        {
            var predicate = ExpressionBuilder.GetExpression<T>(filters);
            return predicate != null ? source.Where(predicate) : source;
        }

        public static async Task<IHtmlContent> StaticText(this IViewComponentHelper Component, string value)
        {
            return await Component.InvokeAsync("LanguageText", new { key = value, type = 1 });
        }

        public static string StaticText2(this IViewComponentHelper Component, string value)
        {
            var output = Component.InvokeAsync("LanguageText", new { key = value, type = 1 }).Result;
            return output.ToString();
        }

        public static async Task<IHtmlContent> DynamicText(this IViewComponentHelper Component, string value)
        {
            return await Component.InvokeAsync("LanguageText", new { key = value, type = 2 });
        }

        public static async Task<IHtmlContent> UserInfo(this IViewComponentHelper Component, Guid guid, string field)
        {
            return await Component.InvokeAsync(field, new { guid = guid }); ;
        }

        public static bool IsNumeric(this string text) => double.TryParse(text, out _);

        public static bool IsDate(this string text) => DateTime.TryParse(text, out _);
    }


}

