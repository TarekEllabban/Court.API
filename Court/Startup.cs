using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Court.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Court.API.IServices;
using Court.API.Services;
using Court.Entities.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
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
            services.AddMvc(o =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                o.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<CourtContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CourtConnection")));
            services.AddDbContext<CourtIdentityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CourtConnection")));

            #region Identity Registeration [IdentityUser]
            //services.AddIdentityCore<IdentityUser>(options => { });
            //services.AddScoped<IUserStore<IdentityUser>, UserOnlyStore<IdentityUser, CourtIdentityContext>>();
            #endregion

            #region Identity Registeration [IdentityUser, IdentityRole]
            services.AddIdentity<IdentityUser, IdentityRole>(options => { })
                .AddEntityFrameworkStores<CourtIdentityContext>()
                .AddDefaultTokenProviders();
            #endregion

            #region Authentication
            var token = Configuration.GetSection("TokenManagement").Get<TokenManagementViewModel>();
            var secret = Encoding.ASCII.GetBytes(token.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidIssuer = token.Issuer,
                    ValidAudience = token.Audience,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });
            #endregion

            #region DI
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<Identity.IServices.IUserManager, Identity.Services.UserManager>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            #endregion
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
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
