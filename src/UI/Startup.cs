using System;
using MailSender.MailKit;

using MailTemplates.Razor;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using RazorEngineCore;

using TemplateMailSender.Core.MailSender;
using TemplateMailSender.Core.TemplateBuilder;
using UI.DataModels;
using UI.Services;

namespace UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // set AddRazorPages and ServerSideBlazor first to override registered by them services
            services.AddRazorPages();
            // AddServerSideBlazor register it self AuthenticationStateProvider if it in down of list custom provider will not be used
            services.AddServerSideBlazor();

            var urls = Configuration.GetSection(nameof(Urls)).Get<Urls>();

            // register authentication state provider in that manner to simplify it access
            services.AddScoped<WebApiAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<WebApiAuthenticationStateProvider>());

            services.AddHttpClient<AuthenticationService>(client => client.BaseAddress = new Uri(urls.Identity));

            services.AddSingleton<IRazorEngine, RazorEngine>();
            services.AddSingleton<ITemplateBuilder, MailContentBuilder>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<ModelsGenerator>();
            services.AddSingleton<TemplatesRepository>();

            services.AddAuthentication();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
