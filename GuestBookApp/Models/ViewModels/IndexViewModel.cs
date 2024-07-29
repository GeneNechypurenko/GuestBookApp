namespace GuestBookApp.Models.ViewModels
{
	public class IndexViewModel
	{
		public string Username { get; set; }
		public IEnumerable<Message> Messages { get; set; }
	}
}
