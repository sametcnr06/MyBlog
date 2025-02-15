using Microsoft.EntityFrameworkCore;
using MyBlog.Business.Abstract;
using MyBlog.DataAccess.Contexts;
using MyBlog.Entities;
using MyBlog.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FavoriteManager : IFavoriteService
{
    private readonly MyBlogContext _context;

    public FavoriteManager(MyBlogContext context)
    {
        _context = context;
    }

    // 📌 Kullanıcıyı takip etme işlemi
    public async Task<bool> FollowUserAsync(string followerId, string followedUserId)
    {
        bool alreadyFollow = await _context.Favorites
            .AnyAsync(f => f.FollowerId == followerId && f.FollowedUserId == followedUserId);

        if (!alreadyFollow)
        {
            var fav = new Favorite
            {
                FollowerId = followerId,
                FollowedUserId = followedUserId,
                DateAdded = DateTime.Now
            };
            await _context.Favorites.AddAsync(fav);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        return false;
    }

    // 📌 Kullanıcının bir yazarı takipten çıkması
    public async Task<bool> UnfollowUserAsync(string followerId, string followedUserId)
    {
        var fav = await _context.Favorites
            .FirstOrDefaultAsync(f => f.FollowerId == followerId && f.FollowedUserId == followedUserId);
        if (fav != null)
        {
            _context.Favorites.Remove(fav);
            return await _context.SaveChangesAsync() > 0;
        }
        return false;
    }

    // 📌 Takip edilen kullanıcıları getir
    public async Task<List<ApplicationUser>> GetFollowedUsersAsync(string followerId)
    {
        var favs = await _context.Favorites
            .Include(f => f.FollowedUser)
            .Where(f => f.FollowerId == followerId)
            .ToListAsync();

        return favs.Select(f => f.FollowedUser).Where(u => u != null).ToList();
    }

    // 📌 Kullanıcının belirli bir yazarı takip edip etmediğini kontrol et
    public async Task<bool> IsFollowingAsync(string followerId, string authorId)
    {
        return await _context.Favorites
            .AnyAsync(f => f.FollowerId == followerId && f.FollowedUserId == authorId);
    }
}
