namespace BankingSystem.Core.Model
{
    public class User
    {
		public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
		public string PersonalNumber { get; set; }
		public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public User(string id , string name , string lastname,  string email , string personalnumber , string password, DateTime birthDate)
		{
            Id = id;
			Name = name;
            LastName = lastname;
            Email=email;
            PersonalNumber = personalnumber;
            Password = password;
            BirthDate = birthDate;
		}
	}
}