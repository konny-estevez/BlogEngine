using Microsoft.AspNetCore.Cors.Infrastructure;

namespace WebApi.Configurations
{
    public class Cors
    {
        public static readonly string CorsPolicy = "AllowCorsPolicy";

        public static Action<CorsOptions> GetOptions()
        {
            return options =>
            {
                options.AddPolicy(CorsPolicy, builder =>
                {
                    builder.WithOrigins("https://localhost")
                    .AllowCredentials()
                    .WithMethods("POST", "GET", "PUT", "DELETE", "UPDATE", "OPTIONS", "PATCH")
                    .WithHeaders("content-type", "apiKey", "accept", "Authorization");
                });
            };
        }
    }
}
