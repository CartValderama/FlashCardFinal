using Microsoft.AspNetCore.Identity;

namespace FlashCardDemo.Models
{
    public class FlashCardDemoUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
