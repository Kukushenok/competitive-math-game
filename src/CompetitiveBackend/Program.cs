
using CompetitiveBackend.Controllers;
using CompetitiveBackend.SolutionInstaller;
using ImageProcessorRealisation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using RepositoriesRealisation;
using ServicesRealisation;

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
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            //app.UseHttpsRedirection();
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseExceptionHandler();
            app.MapControllers();
            app.Services.InitializeCompetitiveBackendSolution();
            app.Run();
        }
    }
    // chatgpt told me to do this
    public partial class Program { }
}