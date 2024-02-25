using BankingSystem.Core.Data;
using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.BankAccounts;
using BankingSystem.Core.Features.Cards;
using BankingSystem.Core.Shared;
using Classroom.TodoWithAuth.Auth.Db;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Text;

namespace BankingSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            

            // Token-ის შემქნელი
            var issuer = "myapp.com";

            //Token-ის აუდიტორია
            var audience = "myapp.com";

            // Token-ის გასაღები 
            var secretKey = builder.Configuration["JwtTokenSecretKey"]!;

            // Token-ის ვალიდაციის პარამეტრები, რის მიხედვითაც aspn.net მოახდენს ვალიდაციას.

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = false,
                ValidIssuer = issuer,
                ValidAudience = audience,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            };

            builder.Services.AddTransient<JwtTokenGenerator>();

            var connectionString = builder.Configuration.GetConnectionString("Zangura")!;


            builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            builder.Services.AddScoped<IBankAccountService, BankAccountService>();
            builder.Services.AddScoped<IDataManager, DataManager>();
			builder.Services.AddScoped<ICardRepository, CardRepository>(); 
			builder.Services.AddScoped<ICardService, CardService>();



			// საჭირო სერვისების IoC-ში რეგისტრაცია

			builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => { options.TokenValidationParameters = tokenValidationParameters; });

            builder.Services
         .AddDbContext<AppDbContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString(Environment.UserName)));

            // Policy-ს შექმნა და კონტეინერში დამატება
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MyApiUserPolicy", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "user");
                });

                options.AddPolicy("OperatorPolicy", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "operator");
                });
            });

            //UserEntity და RoleEntity კლასების მიხედვით მოხდება ბაზაში ცხრილების შექმნა
            builder.Services
                .AddIdentity<UserEntity, RoleEntity>(o =>
                {
                    o.Password.RequireDigit = true;
                    o.Password.RequireLowercase = true;
                    o.Password.RequireUppercase = true;
                    o.Password.RequireNonAlphanumeric = true;
                    o.Password.RequiredLength = 8;
                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<JwtTokenGenerator>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();

            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            app.Run();
        }
    }
}