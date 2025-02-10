using MyBlog.Entities.Identity;
using System;

namespace MyBlog.Entities
{
    public class Favorite : BaseEntity
    {
        public string FollowerId { get; set; } // Takip eden kullanıcının ID'si.
        public ApplicationUser Follower { get; set; } // Takip eden kullanıcı.

        public string FollowedUserId { get; set; } // Takip edilen kullanıcının ID'si.
        public ApplicationUser FollowedUser { get; set; } // Takip edilen kullanıcı.

        public DateTime DateAdded { get; set; } = DateTime.Now; // Takip etme işleminin yapıldığı tarih.
    }
}
