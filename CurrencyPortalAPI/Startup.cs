﻿
using CurrencyPortalAPI.ErrorHandling;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using UserManagement;

namespace CurrencyPortalAPI
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
			services.AddMvc(SetupFilters)
					.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			var appConfig = Configuration.GetAppConfig();

			services.ConfigureUserAuthentication(appConfig.JwtSettings);

			services.ConfigureBussinesServices(appConfig.ClientConfig);

			services.ConfigureReposDI(appConfig.ConnectionSettings);
		}



		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseAuthentication();

			app.UseMvc();
		}

		private void SetupFilters(MvcOptions mvcOptions)
			=> mvcOptions.Filters.Add(new InvalidIdentityDataFilter());
	}
}
