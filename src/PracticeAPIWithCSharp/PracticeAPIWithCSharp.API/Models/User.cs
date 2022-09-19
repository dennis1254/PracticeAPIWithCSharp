namespace PracticeAPIWithCSharp.API.Models
{
	public class User
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string RefreshToken { get; set; }
		public bool IsActive { get; set; }

	}
}
