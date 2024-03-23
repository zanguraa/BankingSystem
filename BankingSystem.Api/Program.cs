﻿using BankingSystem.Api.Middlewares;
using BankingSystem.Core.Data;
using BankingSystem.Core.Data.Entities;
using BankingSystem.Core.Features.Atm.CardAuthorization;
using BankingSystem.Core.Features.Atm.ChangePin;
using BankingSystem.Core.Features.Atm.ViewBalance;
using BankingSystem.Core.Features.Atm.WithdrawMoney.WithdrawMoneyRepository;
using BankingSystem.Core.Features.Atm.WithdrawMoney.WithdrawMoneyServices;
using BankingSystem.Core.Features.BankAccounts.AddFunds;
using BankingSystem.Core.Features.BankAccounts.BankAccountRepositories;
using BankingSystem.Core.Features.BankAccounts.BankAccountsServices;
using BankingSystem.Core.Features.Cards;
using BankingSystem.Core.Features.Reports;
using BankingSystem.Core.Features.Transactions.Currency;
using BankingSystem.Core.Features.Transactions.TransactionServices;
using BankingSystem.Core.Features.Transactions.TransactionsRepositories;
using BankingSystem.Core.Features.Users;
using BankingSystem.Core.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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

            builder.Services.AddScoped<IAddFundsService, AddFundsService>();
            builder.Services.AddScoped<IAddFundsRepository, AddFundsRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            builder.Services.AddScoped<IBankAccountService, BankAccountService>();
            builder.Services.AddScoped<ICardRepository, CardRepository>();
            builder.Services.AddScoped<ICardService, CardService>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
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
            builder.Services.AddScoped<ITransactionServiceValidator, TransactionServiceValidator>();
            builder.Services.AddScoped<IWithdrawMoneyServiceValidator, WithdrawMoneyServiceValidator>();


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

                options.AddPolicy("CardHolder", policy =>
                    policy.RequireClaim("CardHolderStatus", "Active")
                );
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