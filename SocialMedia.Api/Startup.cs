using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Infrastructure.Extensions;
using SocialMedia.Infrastructure.Filters;
using System;
using System.Reflection;
using System.Text;

namespace SocialMedia.Api
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            //This is to use ApiController without model validation feature
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                //options.SuppressModelStateInvalidFilter = true;
            });

            services.AddOptions(Configuration);
            //services.Configure<PaginationOptions>(Configuration.GetSection("Pagination"));
            //services.Configure<PasswordOptions>(Configuration.GetSection("PasswordOptions"));

            services.AddDbContexts(Configuration);
            //services.AddDbContext<SocialMediaContext>(options =>
            //    options.UseSqlServer(Configuration.GetConnectionString("SocialMedia"))
            //);            

            services.AddServices();
            //services.AddTransient<IPostService, PostService>();
            //services.AddTransient<ISecurityService, SecurityService>();
            //services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddSingleton<IPasswordService, PasswordService>();
            //services.AddSingleton<IUriService>(provider =>
            //{
            //    var accesor = provider.GetRequiredService<IHttpContextAccessor>();
            //    var request = accesor.HttpContext.Request;
            //    var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
            //    return new UriService(absoluteUri);
            //});

            services.AddSwagger($"{ Assembly.GetExecutingAssembly().GetName().Name }.xml");
            //services.AddSwaggerGen(doc =>
            //{
            //    doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media API", Version = "v1" });
                
            //    var xmlFile = $"{ Assembly.GetExecutingAssembly().GetName().Name }.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);                
            //    doc.IncludeXmlComments(xmlPath);
            //});

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                };
            });

            services.AddMvc(options =>
            {
                options.Filters.Add<ValidationFilters>();
            }).AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("../swagger/v1/swagger.json", "Social Media API v1");
                options.RoutePrefix = string.Empty; // Uncomment for IIS Express
                //options.RoutePrefix = string.Empty;  //Commented when using local IIS
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
