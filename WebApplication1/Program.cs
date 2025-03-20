namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            var app = builder.Build();
            app.UseStaticFiles();
            app.UseRouting();
            app.Use(async (context, next) =>
            {
                Endpoint endpoint = context.GetEndpoint();
                if (endpoint is null)
                    await context.Response.WriteAsync("PAGE NOT FOUND!");
                await next();

            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to the Home Page!");
                });

                endpoints.MapGet("/Home", async context =>
                {
                    await context.Response.WriteAsync("Welcome To Home Page");
                });
                endpoints.MapGet("/Products/{id?}", async context =>
                {
                    var idData = context.Request.RouteValues["id"];
                    if (idData is null)
                        await context.Response.WriteAsync("No Product Id Found");
                    else
                        await context.Response.WriteAsync($"Product Id: {idData}");

                });
            });

            app.MapControllerRoute(
            name: "default",
            pattern: "/{Controller=Home}/{Action=Index}",
            defaults: new { Controller = "Home", Action = "Index" });

            app.Run();
        }
    }
}
