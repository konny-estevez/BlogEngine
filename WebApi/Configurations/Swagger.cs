using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Configurations
{
    public class Swagger
    {
        private static readonly int MajorVersion = 1;
        private static readonly int MinorVersion = 0;
        public static Action<SwaggerGenOptions> GetOptions()
        {
            return options =>
            {
                options.CustomSchemaIds(x => x.FullName);
                options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.ActionDescriptor.RouteValues["action"]}");
                options.SwaggerDoc($"v{MajorVersion}", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Group Management",
                    Version = $"{MajorVersion}.{MinorVersion}",
                    Description = "Web API",
                });

                // Configure Swagger to use the xml documentation file
                var xmlFile = Path.Combine(AppContext.BaseDirectory, "WebApi.xml");
                options.IncludeXmlComments(xmlFile);
            };
        }
    }
}
