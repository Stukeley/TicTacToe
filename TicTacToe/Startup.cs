﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using TicTacToe.Extensions;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe
{
	public class Startup
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddLocalization(options =>
			options.ResourcesPath = "Localization");

			services.AddMvc();
			services.AddSingleton<IUserService, UserService>();
			services.AddRouting();

			services.AddSession(o =>
			{
				o.IdleTimeout = TimeSpan.FromMinutes(30);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
				return context.Response.WriteAsync($"Użytkownik {firstName} {lastName} został pomyślnie utworzony.");
			});

			var newUserRoutes = routeBuilder.Build();
			app.UseRouter(newUserRoutes);

			var options = new RewriteOptions().AddRewrite("NewUser", "/UserRegistration/Index", false);
			app.UseRewriter(options);

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
				routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
			});

			app.UseStatusCodePages("text/plain", "Błąd HTTP - kod odpowiedzi: {0}");
		}
	}
}