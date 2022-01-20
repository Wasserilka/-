using Lesson_1_2;
using Lesson_1_2.Controllers;
using Lesson_1_2.Repositories;
using Lesson_1_2.Models;
using Lesson_1_2.Interfaces;
using AutoMapper;
using FluentMigrator;
using FluentMigrator.Postgres;
using FluentMigrator.Runner;
using Npgsql;
using System;
using System.Collections.Generic;
using Dapper;
using System.Data.SqlClient;

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

            using (var connection = new ConnectionManager().GetOpenedConnection())
            {
                connection.Execute("CREATE TABLE IF NOT EXISTS cards(id SERIAL PRIMARY KEY, number BIGINT, holdername TEXT, expirationdate BIGINT, type TEXT)");
            }

            var mapperConfiguration = new MapperConfiguration(mp => mp.AddProfile(new MapperProfile()));
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
