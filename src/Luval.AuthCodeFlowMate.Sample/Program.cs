using Luval.AuthCodeFlowMate.Infrastructure.Configuration;
using Luval.AuthCodeFlowMate.Infrastructure.Data;
using Luval.AuthCodeFlowMate.Sample.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Luval.AuthCodeFlowMate.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = builder.Configuration;
            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddFluentUIComponents();

            // For controllers
            builder.Services.AddLogging();
            builder.Services.AddControllers();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddAuthCodeFlowMate(config["OAuthProviders:Google:ClientId"], config["OAuthProviders:Google:ClientSecret"], "https://localhost:7047/api/googlecodeflow/callback");
            //

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.MapControllers();
            app.UseRouting();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            //Create the DB
            var db = new SqliteAuthCodeFlowDbContext();
            db.Database.EnsureCreated();

            app.Run();
        }
    }
}
