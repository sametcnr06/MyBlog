using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities
{
    
    // Blog yazıları ile kategoriler arasındaki ilişkileri temsil eder.
    
    public class PostCategory : IBaseEntity
    {
        public int PostId { get; set; } // İlişkilendirilmiş yazının ID'si.
        public Post Post { get; set; } // İlişkilendirilmiş yazı.

        public int CategoryId { get; set; } // İlişkilendirilmiş kategorinin ID'si.
        public Category Category { get; set; } // İlişkilendirilmiş kategori.
    }
}
