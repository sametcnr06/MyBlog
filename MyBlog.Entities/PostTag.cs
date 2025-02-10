using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Entities
{
    /// <summary>
    /// Blog yazıları ile etiketler arasındaki ilişkileri temsil eder.
    /// </summary>
    public class PostTag
    {
        public int PostId { get; set; } // İlişkilendirilmiş yazının ID'si.
        public Post Post { get; set; } // İlişkilendirilmiş yazı.

        public int TagId { get; set; } // İlişkilendirilmiş etiketin ID'si.
        public Tag Tag { get; set; } // İlişkilendirilmiş etiket.
    }
}

