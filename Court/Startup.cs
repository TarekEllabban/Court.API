using System;
using Court.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Court.API.IServices;
using Court.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Court.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Court
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
            #region AddMvc
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            #endregion

            #region DB Context
            services.AddDbContext<CourtContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CourtConnection")));
            services.AddDbContext<CourtIdentityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CourtConnection")));
            #endregion

            #region Identity Registeration [IdentityUser]
            //services.AddIdentityCore<IdentityUser>(options => { });
            //services.AddScoped<IUserStore<IdentityUser>, UserOnlyStore<IdentityUser, CourtIdentityContext>>();
            #endregion

            #region Identity Registeration [IdentityUser, IdentityRole]
            services.AddIdentity<IdentityUser, IdentityRole>(options => { })
                .AddEntityFrameworkStores<CourtIdentityContext>()
                .AddDefaultTokenProviders();
            #endregion

            #region Identity Server
            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                //.AddTestUsers(Config.GetUsers());
               .AddAspNetIdentity<IdentityUser>();
            #endregion

            #region Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // base-address of your identityserver
                options.Authority = Configuration["AuthorityUrl"];

                // name of the API resource
                options.Audience = "Court.API";

                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = Configuration["AuthorityUrl"],
                    ValidAudience = "Court.API",
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion      

            #region DI
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<Identity.IServices.IUserManager, Identity.Services.UserManager>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            #endregion  

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            //app.UseHttpsRedirection();
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
