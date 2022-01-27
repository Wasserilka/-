using Lesson_1_2.DAL.Repositories;
using Lesson_1_2.Connection;
using Lesson_1_2.Security.Service;
using Lesson_1_2.Validation.Validators;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Dapper;
using System.Text;
using Microsoft.OpenApi.Models;

namespace Lesson_1_2
{
    public class StartUp
    {
        public StartUp(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IConnectionManager, ConnectionManager>();

            services.AddSingleton<ICardsRepository, CardsRepository>();
            services.AddSingleton<IUsersRepository, UsersRepository>();

            services.AddSingleton<IAuthService, AuthService>();
            services.AddCors();

            services.AddScoped<IRegisterUserRequestValidator, RegisterUserRequestValidator>();
            services.AddScoped<IAuthenticateUserRequestValidator, AuthenticateUserRequestValidator>();

            services.AddScoped<ICreateCardRequestValidator, CreateCardRequestValidator>();
            services.AddScoped<IUpdateCardRequestValidator, UpdateCardRequestValidator>();
            services.AddScoped<IDeleteCardRequestValidator, DeleteCardRequestValidator>();
            services.AddScoped<IGetByIdCardRequestValidator, GetByIdCardRequestValidator>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthService.SecretCode)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.Execute("CREATE TABLE IF NOT EXISTS cards(id SERIAL PRIMARY KEY, number BIGINT, holdername TEXT, expirationdate BIGINT, type TEXT)");
                connection.Execute("CREATE TABLE IF NOT EXISTS users(id SERIAL PRIMARY KEY, login TEXT, password TEXT, token TEXT, expirationdate BIGINT)");
            }

            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => 
            { 
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
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
                        Array.Empty<string>()
                    }
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
