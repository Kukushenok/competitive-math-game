
using CompetitiveBackend.Controllers;
using ImageProcessorRealisation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using RepositoriesRealisation;
using ServicesRealisation;

namespace CompetitiveBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IAuthorizationHandler, SessionTokenAuthorizationHandler>();
            builder.Services.AddScoped<IAuthenticationHandler, SessionTokenAuthenticationHandler>();
            builder.Services.AddAuthentication("SessionToken")
                .AddScheme<AuthOptions, SessionTokenAuthenticationHandler>("SessionToken", null);
            builder.Services.AddAuthorization((options) =>
            {
                options.AddPolicy("Player", (pol) => pol.Requirements.Add(new PlayerTokenRequirement()));
                options.AddPolicy("Admin", (pol) => pol.Requirements.Add(new AdminTokenRequirement()));
            });
            // The SIN of us. Pray for it to work blud.
            builder.Services.AddRepositories();

            builder.Services.AddServicesWhichAreDone(); // some of them are not done.

            builder.Services.AddMajickImageRescaler();
            //
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}