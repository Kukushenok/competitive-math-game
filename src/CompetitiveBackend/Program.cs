using System.Reflection;
using CompetitiveBackend.Controllers;
using CompetitiveBackend.SolutionInstaller;
using Microsoft.OpenApi.Models;
using PrometheusCollectorSetupper;
using TelemetryInstaller;

namespace CompetitiveBackend
{
    public partial class Program
    {
        private static void SetupSwagger(IServiceCollection coll)
        {
            coll.AddSwaggerGen(setup =>
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

        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCompetitiveBackendSolution();
            builder.Services.AddExceptionHandler<BaseControllerErrorHandler>();
            builder.Services.AddProblemDetails();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddPrometheusMetrics();
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddTelemetry(builder.Environment);
            SetupSwagger(builder.Services);

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapPost("/competition_ensurance/{minutes}", async (int minutes) => await BenchmarkFakerSetupper.BenchmarkBypass(app.Services, minutes));
            app.UsePrometheusMetrics();
            app.UseExceptionHandler();
            app.MapControllers();
            app.Services.InitializeCompetitiveBackendSolution();
            app.Run();
        }
    }

    // chatgpt told me to do this
    public partial class Program
    {
    }
}