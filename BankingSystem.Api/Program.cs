
using BankingSystem.Core.Data;
using BankingSystem.Core.Interfaces;
using BankingSystem.Core.Repositories;
using BankingSystem.Core.Services;

namespace BankingSystem.Api
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
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<IOperatorRepository, OperatorRepository>();
            builder.Services.AddTransient<IDatamanager, DataManager>();
            builder.Services.AddTransient<IOperatorServices, OperatorServices>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}