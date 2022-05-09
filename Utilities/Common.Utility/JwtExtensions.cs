using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// Extension class for JWT
    /// </summary>
    public static class JwtExtensions
    {
        /// <summary>
        /// Create JWT Token Validation Mehanism
        /// </summary>
        /// <param name="services"></param>
        /// <param name="environment"></param>
        /// <param name="builder"></param>
        public static void AddJwtAuthentication(this IServiceCollection services,
            IWebHostEnvironment environment,
            IConfiguration config)
        {
            // Hard Coded file should be ENVironment Specific
            services
                .AddAuthentication(options =>
               {
                   options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               })
                .AddJwtBearer("auth_bearer", options =>
                 {
                     options.RequireHttpsMetadata = true;
                     options.SaveToken = true;

                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                        ValidateIssuer = true, // validate the server
                        ValidateAudience = true, // Validate the recipient of token is authorized to receive
                        ValidateLifetime = true, // Check if token is not expired and the signing key of the issuer is valid 
                        ValidateIssuerSigningKey = true, // Validate signature of the token 

                        //Issuer and audience values are same as defined in generating Token
                        ValidIssuer = config["Jwt:Issuer"], // stored in appsetting file
                        ValidAudience = config["Jwt:Audience"], // stored in appsetting file
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])), // stored in appsetting file
                    };
                 });
        }
    }
}
