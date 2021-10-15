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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
                opt.UseSqlServer(Configuration.GetValue<string>("Databases:Contactr")));
            
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers()
                .AddNewtonsoftJson();
            
            services.AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration.GetSection("Jwt:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contactr", Version = "v1" }); });
            
            // Services
            services.AddHttpClient();
            
            services
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
                .AddScoped<IUserRepository, UserRepository>()
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contactr v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}