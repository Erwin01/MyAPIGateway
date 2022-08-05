using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyAPIGateway.Aggregators;
using MyAPIGateway.Handlers;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

namespace MyAPIGateway
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

            #region [ AUTHENTICATION ]
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("MyAnonymousSecuredSecretKey")),
                            ClockSkew = new System.TimeSpan(0)
                        };
                    });
            #endregion


            services.AddOcelot()
                    .AddDelegatingHandler<RemoveEncodingDeletingHandler>(true) //true if is global
                    .AddDelegatingHandler<BlackListHandler>()
                    .AddSingletonDefinedAggregator<UsersPostsAggregator>();   //Aggregator must be class name
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseAuthentication();
            app.UseOcelot().Wait();
           
        }
    }
}
