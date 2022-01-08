using System;
using System.Configuration;

using MailScheduler.Jobs;
using MailScheduler.Services;
using MailSender.MailKit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Quartz;

using TemplateMailSender.Core;
using TemplateMailSender.Core.MailSender;

namespace MailScheduler
{
	public class Startup
	{
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
        {
            var domainConfiguration = _configuration.GetSection("Domain");
            services.Configure<EmailConfiguration>(domainConfiguration);
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<EmailsRepository>();
            services.AddSingleton<AuthorizationHelper>();

            services.AddQuartz(q =>
            {
                q.UseMicrosoftDependencyInjectionJobFactory();

                q.ScheduleJob<MailSendJob>(trigger => trigger
                    .StartNow()
                    .WithSimpleSchedule(builder => builder.RepeatForever().WithIntervalInMinutes(3))
                );
            });

            services.AddQuartzHostedService(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });

            services.RegisterCors();

            services.ConfigureAuthentication(_configuration);

            services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "MailScheduler", Version = "v1" });

                c.ConfigureSwaggerAuthentication();
            });
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MailScheduler v1"));

			app.UseRouting();

            app.UseCors(JwtSettings.AuthPolicy);
            app.UseAuthentication();
            app.UseAuthorization();

			app.UseEndpoints(endpoints => endpoints.MapControllers());
		}
	}
}
