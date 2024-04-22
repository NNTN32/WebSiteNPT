using Microsoft.AspNetCore.Identity;
namespace WebShopNPT.Models
{
    public class User : IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
    }
}
