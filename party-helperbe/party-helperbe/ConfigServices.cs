using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace party_helperbe
{
    public static class ConfigServices
    {
        public static void AddConfig(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            services.AddDbContext<PgSQLDbContext>(opt => opt.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Environment.GetEnvironmentVariable("ISSUER"),
                    ValidAudience=Environment.GetEnvironmentVariable("AUDIENCE"),
                    IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("KEY"))),
                    ValidateIssuerSigningKey =true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
        
     
                };
            });


        }
    }
}
