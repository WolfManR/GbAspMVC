using MailSender.MailKit;

using MailTemplates.Razor;

using Microsoft.AspNetCore.Builder;
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
            var urls = Configuration.GetSection(nameof(Urls)).Get<Urls>();

            services.AddSingleton<IRazorEngine, RazorEngine>();
            services.AddSingleton<ITemplateBuilder, MailContentBuilder>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<ModelsGenerator>();
            services.AddSingleton<TemplatesRepository>();
            services.AddRazorPages();
            services.AddServerSideBlazor();

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
