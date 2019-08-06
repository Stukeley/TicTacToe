﻿using Halcyon.Web.HAL.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using TicTacToe.Data;
using TicTacToe.Extensions;
using TicTacToe.Filters;
using TicTacToe.Models;
using TicTacToe.Options;
using TicTacToe.Services;
using TicTacToe.ViewEngines;

namespace TicTacToe
{
	public class Startup
	{
		public IConfiguration _configuration { get; }
		public IHostingEnvironment _hostingEnvironment { get; }
		public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
		{
			_configuration = configuration;
			_hostingEnvironment = hostingEnvironment;
		}

		public void ConfigureCommonServices(IServiceCollection services)
		{
			services.AddLocalization(options => options.ResourcesPath = "Localization");
			services.AddMvc(o =>
			{
				o.Filters.Add(typeof(DetectMobileFilter));

				o.OutputFormatters.RemoveType<JsonOutputFormatter>();
				o.OutputFormatters.Add(new JsonHalOutputFormatter(new string[] { "application/hal+json", "application/vnd.example.hal+json", "application/vnd.example.hal.v1+json" }));
			}).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix, options => options.ResourcesPath = "Localization").AddDataAnnotationsLocalization();

			services.AddSingleton<IUserService, UserService>();
			services.AddSingleton<IGameInvitationService, GameInvitationService>();
			services.AddSingleton<IGameSessionService, GameSessionService>();

			var connectionString = _configuration.GetConnectionString("DefaultConnection");
			services.AddEntityFrameworkSqlServer().AddDbContext<GameDbContext>((serviceProvider, options) => options.UseSqlServer(connectionString)
			.UseInternalServiceProvider(serviceProvider));

			var dbContextOptionsBuilder = new DbContextOptionsBuilder<GameDbContext>().UseSqlServer(connectionString);
			services.AddSingleton(dbContextOptionsBuilder.Options);

			services.Configure<EmailServiceOptions>(_configuration.GetSection("Email"));
			services.AddEmailService(_hostingEnvironment, _configuration);
			services.AddTransient<IEmailTemplateRenderService, EmailTemplateRenderService>();
			services.AddTransient<EmailViewEngine, EmailViewEngine>();

			services.AddRouting();
			services.AddSession(o =>
			{
				o.IdleTimeout = TimeSpan.FromMinutes(30);
			});
		}

		public void ConfigureDevelopmentServices(IServiceCollection services)
		{
			ConfigureCommonServices(services);
		}

		public void ConfigureStagingServices(IServiceCollection services)
		{
			ConfigureCommonServices(services);
		}

		public void ConfigureProductionServices(IServiceCollection services)
		{
			ConfigureCommonServices(services);
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseBrowserLink();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();
			app.UseSession();

			var routeBuilder = new RouteBuilder(app);
			routeBuilder.MapGet("CreateUser", context =>
			{
				var firstName = context.Request.Query["firstName"];
				var lastName = context.Request.Query["lastName"];
				var email = context.Request.Query["email"];
				var password = context.Request.Query["password"];
				var userService = context.RequestServices.GetService<IUserService>();
				userService.RegisterUser(new UserModel { FirstName = firstName, LastName = lastName, Email = email, Password = password });
				return context.Response.WriteAsync($"Uzytkownik {firstName} {lastName} zostal pomyslnie utworzony.");
			});
			var newUserRoutes = routeBuilder.Build();
			app.UseRouter(newUserRoutes);

			app.UseWebSockets();
			app.UseCommuncationMiddleware();

			var supportedCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
			var localizationOptions = new RequestLocalizationOptions
			{
				DefaultRequestCulture = new RequestCulture("pl-PL"),
				SupportedCultures = supportedCultures,
				SupportedUICultures = supportedCultures
			};

			localizationOptions.RequestCultureProviders.Clear();
			localizationOptions.RequestCultureProviders.Add(new CultureProviderResolverService());

			app.UseRequestLocalization(localizationOptions);

			app.UseMvc(routes =>
			{
				routes.MapRoute(name: "areaRoute",
						template: "{area:exists}/{controller=Home}/{action=Index}");

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});

			app.UseStatusCodePages("text/plain", "Blad HTTP - kod odpowiedzi: {0}");

			using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				scope.ServiceProvider.GetRequiredService<GameDbContext>().Database.Migrate();
			}
		}
	}
}
