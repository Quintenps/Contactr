using System;
using System.Collections.Generic;
using System.Text;
using Contactr.Factories;
using Contactr.Factories.Interfaces;
using Contactr.Persistence;
using Contactr.Persistence.Repositories;
using Contactr.Persistence.Repositories.Interfaces;
using Contactr.Services.AuthService;
using Contactr.Services.CardService;
using Contactr.Services.ConnectionService;
using Contactr.Services.DatastoreService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace Contactr
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
            services.AddDbContextPool<AppDbContext>(opt =>
                opt.UseSqlServer(Configuration.GetValue<string>("Databases:contactr")));

            services.AddAutoMapper(typeof(Startup));
            
            services.AddControllers().AddNewtonsoftJson(n => n.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contactr", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "Open Id" }
                            },
                            AuthorizationUrl = new Uri(Configuration["Auth0:authority"] + "authorize?audience=" + Configuration["Auth0:audience"])
                        }
                    }
                });
            });
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = Configuration["Auth0:authority"];
                options.Audience = Configuration["Auth0:audience"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth0:clientSecret"]))
                };
            });
            
            // Services
            services.AddHttpClient();
            services
                .AddSingleton<IMemoryCache, MemoryCache>()

                // Services
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<ICardService, CardService>()
                .AddScoped<ISyncService, SyncService>()
                .AddScoped<IDatastoreService, DatastoreService>()
                .AddScoped<IConnectionService, ConnectionService>()

                // Factories
                .AddScoped<IUserFactory, UserFactory>()
                .AddScoped<ICardFactory, CardFactory>()
                .AddScoped<IConnectionFactory, ConnectionFactory>()
                .AddScoped<IPeopleServiceFactory, PeopleServiceFactory>()
                .AddScoped<IAuthenticationProviderFactory, AuthenticationProviderFactory>()

                // Repositories
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IAddressRepository, AddressRepository>()
                .AddScoped<ICompanyRepository, CompanyRepository>()
                .AddScoped<IConnectionRepository, ConnectionRepository>()
                .AddScoped<IPersonalCardRepository, PersonalCardRepository>()
                .AddScoped<IBusinessCardRepository, BusinessCardRepository>()
                .AddScoped<IAuthenticationProviderRepository, AuthenticationProviderRepository>()
                .AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            using (var context = scope.ServiceProvider.GetService<AppDbContext>())
                context?.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contactr v1");
                        c.OAuthClientId(Configuration["Auth0:clientId"]);
                    }
                );
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}