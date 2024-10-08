using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using minimal_api.Dominio.Interfaces;
using minimal_api.Dominio.Servicos;
using minimal_api.Infraestrutura.Db;
using System.Text;

namespace minimal_api.DependecyInjection
{
    public class DependencyInjection
    {
        public static void AddInfraestructure(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
            var jwtKey = builder.Configuration.GetSection("Jwt").Value;
            builder.Services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("Bearer", opts =>
            {
                opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opts =>
            {
                opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o token JWT desta maneira {Seu Token}"
                });
                opts.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] { }
                        }
                    }
                );
            });
            builder.Services.AddScoped<IAdministradorServico, AdministradorServico>();
            builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();
            builder.Services.AddDbContext<ApiContext>(opts =>
            {
                opts.UseMySQL(connectionString ?? "", opts => opts.MigrationsAssembly("minimal-api"));
            });

            builder.Services.AddAuthorization();
        }
    }
}
