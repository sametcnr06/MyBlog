using Microsoft.AspNetCore.Mvc;

namespace MyBlog.Models.ViewModels
{
    public class KullaniciViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public List<string> Roles { get; set; }
    }
}
