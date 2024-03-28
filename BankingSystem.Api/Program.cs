﻿using BankingSystem.Api.Middlewares;
using BankingSystem.Core.Data;
using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Atm.CardAuthorizations;
using BankingSystem.Core.Features.Atm.ChangePin;
using BankingSystem.Core.Features.Atm.ViewBalance;
using BankingSystem.Core.Features.Atm.WithdrawMoney;
using BankingSystem.Core.Features.BankAccounts.AddFunds;
using BankingSystem.Core.Features.BankAccounts.CreateAccount;
using BankingSystem.Core.Features.Cards.CreateCard;
using BankingSystem.Core.Features.Reports;
using BankingSystem.Core.Features.Transactions.CreateTransactions;
using BankingSystem.Core.Features.Users;
using BankingSystem.Core.Shared;
using BankingSystem.Core.Shared.Services.Currency;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace BankingSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var issuer = "myapp.com";

            var audience = "myapp.com";

            var secretKey = builder.Configuration["JwtTokenSecretKey"]!;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            };

            builder.Services.AddTransient<JwtTokenGenerator>();

            var userName = Environment.UserName;
            var connectionStringName = userName;
            var connectionString = builder.Configuration.GetConnectionString(connectionStringName)!;

            builder.Services.AddSingleton<IDataManager, DataManager>();
            builder.Services.AddSingleton<ISeqLogger, SeqLogger>();
            builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            builder.Services.AddScoped<IAddFundsService, AddFundsService>();
            builder.Services.AddScoped<IAddFundsRepository, AddFundsRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICreateBankAccountsRepository, CreateBankAccountsRepository>();
            builder.Services.AddScoped<ICreateBankAccountsService, CreateBankAccountsService>();
            builder.Services.AddScoped<ICardRepository, CreateCardRepository>();
            builder.Services.AddScoped<ICardService, CreateCardService>();
            builder.Services.AddScoped<ITransactionRepository, CreateTransactionRepository>();
            builder.Services.AddScoped<ICreateTransactionService, CreateTransactionService>();
            builder.Services.AddScoped<ICurrencyConversionService, CurrencyConversionService>();
            builder.Services.AddScoped<ICurrencyConversionRepository, CurrencyConversionRepository>();
            builder.Services.AddScoped<ICardAuthorizationRepository, CardAuthorizationRepository>();
            builder.Services.AddScoped<ICardAuthorizationService, CardAuthorizationService>();
            builder.Services.AddScoped<IChangePinService, ChangePinService>();
            builder.Services.AddScoped<IChangePinRepository, ChangePinRepository>();
            builder.Services.AddScoped<IViewBalanceRepository, ViewBalanceRepository>();
            builder.Services.AddScoped<IViewBalanceService, ViewBalanceService>();
            builder.Services.AddScoped<IWithdrawMoneyRepository, WithdrawMoneyRepository>();
            builder.Services.AddScoped<IWithdrawMoneyService, WithdrawMoneyService>();
            builder.Services.AddScoped<IReportsService, ReportsService>();
            builder.Services.AddScoped<IReportsRepository, ReportsRepository>();
            builder.Services.AddScoped<ITransactionServiceValidator, CreateTransactionServiceValidator>();


            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => { options.TokenValidationParameters = tokenValidationParameters; });

            builder.Services
         .AddDbContext<AppDbContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString(Environment.UserName)));

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("MyApiUserPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimTypes.Role, "user");
                });

                options.AddPolicy("OperatorPolicy", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "operator");
                });

                options.AddPolicy("AtmPolicy", policy =>
               {
                   policy.RequireClaim(ClaimTypes.Role, "atm");

               });
            });

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

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
            app.Run();
        }
    }
}