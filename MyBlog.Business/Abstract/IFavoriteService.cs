using MyBlog.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Business.Abstract
{
    public interface IFavoriteService
    {
        Task<bool> FollowUserAsync(string followerId, string followedUserId);
        Task<bool> UnfollowUserAsync(string followerId, string followedUserId);
        Task<List<ApplicationUser>> GetFollowedUsersAsync(string followerId);
        Task<bool> IsFollowingAsync(string subscriberId, string authorId);
    }
}
