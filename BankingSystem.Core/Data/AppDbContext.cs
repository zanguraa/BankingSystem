using BankingSystem.Core.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace Classroom.TodoWithAuth.Auth.Db
{
    // ავტორიზაციისთვის საჭირო მონაცემთა ბაზის კონფიგურაცია Entity framework-ის გამოყენებით
    public class AppDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // asp.net identity ცხრილების სახელების შეცვლა
            builder.Entity<UserEntity>().ToTable("Users");
            builder.Entity<RoleEntity>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");

            // Seed მონაცემების ჩაწერა
            InsertSeedData(builder);
        }
        private void InsertSeedData(ModelBuilder builder)
        {
            builder.Entity<RoleEntity>().HasData(new[]
            {
        new RoleEntity { Id = 1, Name = "user", NormalizedName = "USER" },
        new RoleEntity { Id = 2, Name = "operator", NormalizedName = "OPERATOR" }
    });

            var userName = "Hackera@gmail.com";
            var password = "Admin@123";
            var operatorUser = new UserEntity
            {
                Id = 1,
                Email = userName,
                UserName = userName,
                FirstName = "Hackera",
                LastName = "Hackerashvili",
                NormalizedEmail = userName.ToUpper(),
                BirthdayDate = new DateTime(1990, 1, 1), // Set a non-null value for BirthdayDate
                PersonalId = "12345678910", // Set a non-null value for PersonalId
                PhoneNumber = "555123456",
            };

            var hasher = new PasswordHasher<UserEntity>();
            operatorUser.PasswordHash = hasher.HashPassword(operatorUser, password);
            builder.Entity<UserEntity>().HasData(operatorUser);

            builder.Entity<IdentityUserRole<int>>().HasData(new[]
            {
        new IdentityUserRole<int> { UserId = 1, RoleId = 2 }
    });
        }
    }
}
