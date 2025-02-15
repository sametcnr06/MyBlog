using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities
{
    
    // Bildirimin türünü belirtir (Örneğin, Yorum, Yeni Yazı, Takip).
    
    public enum NotificationType
    {
        Comment = 1, // Yorum bildirimi
        NewPost = 2, // Yeni yazı bildirimi
        Follow = 3   // Takip bildirimi
    }
}