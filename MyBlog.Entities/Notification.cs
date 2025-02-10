using MyBlog.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities
{
    /// <summary>
    /// Kullanıcılara gönderilen bildirimleri temsil eder.
    /// </summary>
    public class Notification : BaseEntity
    {
        public string UserId { get; set; } // Bildirimi alan kullanıcının ID'si.
        public ApplicationUser User { get; set; } // Bildirimi alan kullanıcı.

        public string Message { get; set; } = string.Empty; // Bildirimin içeriği.

        public NotificationType Type { get; set; } // Bildirimin türü (Yorum, Yeni Yazı, vb.).
        public int? TargetId { get; set; } // Bildirimin hedefinin ID'si (örneğin, yazı ID'si).

        public bool IsRead { get; set; } = false; // Bildirimin okunup okunmadığını belirtir.
    }
}

