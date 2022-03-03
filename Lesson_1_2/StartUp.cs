using Lesson_1_2.DAL.Repositories;
using Lesson_1_2.Connection;
using Lesson_1_2.Security.Service;
using Lesson_1_2.Validation.Validators;
using Lesson_1_2.DAL.Models;
using Lesson_1_2.Consul;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Dapper;
using System.Text;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using Nest;
using Consul;

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
            services.AddSingleton<IPostgreSQLConnectionManager, PostgreSQLConnectionManager>();
            services.AddSingleton<IMongoDBConnectionManager, MongoDBConnectionManager>();
            services.AddSingleton<IElasticSearchConnectionManager, ElasticSearchConnectionManager>();

            services.AddSingleton<IBooksRepository, BooksRepository>();
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

            services.AddScoped<ICreateBookRequestValidator, CreateBookRequestValidator>();
            services.AddScoped<IUpdateBookRequestValidator, UpdateBookRequestValidator>();
            services.AddScoped<IDeleteBookRequestValidator, DeleteBookRequestValidator>();
            services.AddScoped<IGetByTitleBookRequestValidator, GetByTitleBookRequestValidator>();
            services.AddScoped<IGetByAuthorBookRequestValidator, GetByAuthorBookRequestValidator>();

            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = new Uri("http://localhost:8500");
            }));
            services.AddHostedService<ConsulHostedService>();
            services.AddHealthChecks();

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

            using (var connection = new PostgreSQLConnectionManager(Configuration).GetOpenedConnection())
            {
                connection.Execute("CREATE TABLE IF NOT EXISTS cards(id SERIAL PRIMARY KEY, number BIGINT, holdername TEXT, expirationdate BIGINT, type TEXT)");
                connection.Execute("CREATE TABLE IF NOT EXISTS users(id SERIAL PRIMARY KEY, login TEXT, password TEXT, token TEXT, expirationdate BIGINT)");
            }

            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            var elasticSearchConnection = new ElasticSearchConnectionManager(Configuration).GetOpenedConnection();
            if (!elasticSearchConnection.Indices.Exists("books").Exists)
            {
                var elasticSearchIndexSettings = new IndexSettings { NumberOfReplicas = 1, NumberOfShards = 2 };
                var resp = elasticSearchConnection.Indices.Create("books", c => c
                .InitializeUsing(new IndexState { Settings = elasticSearchIndexSettings })
                .Map<Book>(mp => mp.AutoMap()));
            }

            var mongoDBConnection = new MongoDBConnectionManager(Configuration).GetOpenedConnection("local", "books");
            var mongoDBData = mongoDBConnection.Find(new BsonDocument()).ToList()
                .Select(o => BsonSerializer.Deserialize<Book>(o)).ToList();

            foreach (var book in mongoDBData)
            {
                elasticSearchConnection.IndexDocument(book);
            }

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

            app.UseHealthChecks("/healthz");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
