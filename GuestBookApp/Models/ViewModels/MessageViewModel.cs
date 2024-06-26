using System.ComponentModel.DataAnnotations;

namespace GuestBookApp.Models.ViewModels
{
    public class MessageViewModel
    {
        [Required]
        public string Text { get; set; }
    }
}