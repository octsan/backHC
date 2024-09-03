
using ERP_MaxysHC.Maxys.Data;
using ERP_MaxysHC.Maxys.Data.Repositories;
using ERP_MaxysHC.Maxys.Data.RepositoriesSQL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using System.Text;

namespace ERP_MaxysHC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c=>
            {   //Document title and version
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ERP_MaxysHC", Version = "v1" });
                //JWT Authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                //JWT Authorization
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            //COnnection to the database MySQL
            var mySQLConfiguration = new MySQLConfiguration(builder.Configuration.GetConnectionString("MySqlConnection"));
            builder.Services.AddSingleton(mySQLConfiguration);

            //Connection to the database SQL Server
            builder.Services.AddDbContext<MaxysContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            var SQLConfiguration = new SQLConfiguration(builder.Configuration.GetConnectionString("DefaultConnection"));
            builder.Services.AddSingleton(SQLConfiguration);

            // Add services to the container.
            builder.Services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    ).AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            // Dependency Injection MySQL
            builder.Services.AddScoped<IUserRepository, UserRepository>();

            // Dependency Injection SQL Server
            builder.Services.AddScoped<IOCDocumentosRepository, OCDocumentosRepository>();

            //JWT Authentication
            var key = Encoding.ASCII.GetBytes(builder.Configuration["JWT:Key"]);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });


            var app = builder.Build();

            app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

                app.UseSwagger(option =>
                {
                    option.RouteTemplate = "caprom/swagger/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/caprom/swagger/v1/swagger.json", "Maxys");
                    option.RoutePrefix = "caprom";
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
