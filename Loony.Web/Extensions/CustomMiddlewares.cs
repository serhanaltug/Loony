using Loony.Tools;

namespace Loony.Web.Extensions
{
    public static class CustomMiddlewares
    {
        public static IApplicationBuilder CheckLicense(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                bool isActivated = LicenseManager.CheckActivation();
                var url = context.Request.Path.Value;
                if (!url.Contains("License") && isActivated == false)
                {
                    var baseUrl = string.Format("{0}://{1}", context.Request.Scheme, context.Request.Host);
                    context.Response.Redirect(baseUrl + "/License");
                    return;
                }

                await next();
            });

            return app;
        }

    }
}
