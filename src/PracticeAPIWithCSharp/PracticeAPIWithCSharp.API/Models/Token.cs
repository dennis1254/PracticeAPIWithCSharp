using System;

namespace PracticeAPIWithCSharp.API.Models
{
	public class Tokens
	{
		public string AccessToken { get; set; }
		public string RefreshToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
