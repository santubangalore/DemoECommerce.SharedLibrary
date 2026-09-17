using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace eCommerce.SharedLibrary.DependencyInjection
{
    public static class JWTAuthenticationScheme
    {
        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
           
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                   var key=Encoding.UTF8.GetBytes(configuration.GetSection("Authentication:Key").Value!);
                   var issuer = configuration.GetSection("Authentication:Issuer").Value!;
                   var audience = configuration.GetSection("Authentication:Audience").Value!;

                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime= true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer=issuer,
                        ValidAudience=audience,
                        IssuerSigningKey=new SymmetricSecurityKey(key),
                    };
                });
            return services;
        }
    }
}
