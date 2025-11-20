using System.Reflection;
using CompetitiveBackend.Controllers;
using CompetitiveBackend.SolutionInstaller;
using Microsoft.OpenApi.Models;

namespace CompetitiveBackend
{
    public partial class Program
    {
        private static void SetupServices(WebApplicationBuilder builder)
        {
            builder.Services.AddCompetitiveBackendSolution();
            builder.Services.AddExceptionHandler<BaseControllerErrorHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddSwaggerGen(setup =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme,
                    },
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() },
                });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                {
                    setup.IncludeXmlComments(xmlPath);
                }
            });
        }

        public static void SetupApp(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();
            app.MapControllers();
            app.Services.InitializeCompetitiveBackendSolution();
        }

        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            SetupServices(builder);

            WebApplication app = builder.Build();

            SetupApp(app);
            app.Run();
        }
    }

    // chatgpt told me to do this
    public partial class Program
    {
    }
}