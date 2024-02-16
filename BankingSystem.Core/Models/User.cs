﻿namespace BankingSystem.Core.Model
{
    public class User
    {
	

		public string Id { get; set; }
        public string Name { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public User(string name , string lastname,  string email , string password, DateTime birthdate)
		{
			Name = name;
            LastName = lastname;
            Email=email;
            Password = password;
            birthdate = birthdate;
                 
		}
    }
}