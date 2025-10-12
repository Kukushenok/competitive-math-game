
using CompetitiveBackend.Controllers;
using CompetitiveBackend.SolutionInstaller;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using PrometheusCollectorSetupper;
using Repositories.Objects;
using RepositoriesRealisation;
using ServicesRealisation;
using System.Reflection;

namespace CompetitiveBackend
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // The SIN of us. Pray for it to work blud.
            builder.Services.AddCompetitiveBackendSolution();
            builder.Services.AddExceptionHandler<BaseControllerErrorHandler>();
            builder.Services.AddProblemDetails();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddPrometheusMetrics();
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
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                {
                    setup.IncludeXmlComments(xmlPath);
                }

            });

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(
                //    x=>
                //{
                //    x.RoutePrefix = "/api/v1";
                //}
                );

            }
            app.MapPost("/competition_ensurance/{minutes}", async (int minutes) => await BenchmarkFakerSetupper.BenchmarkBypass(app.Services, minutes));
            //app.UseHttpsRedirection();
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UsePrometheusMetrics();

            app.UseExceptionHandler();
            app.MapControllers();
            app.Services.InitializeCompetitiveBackendSolution();
            app.Run();
        }
    }
    // chatgpt told me to do this
    public partial class Program { }
}