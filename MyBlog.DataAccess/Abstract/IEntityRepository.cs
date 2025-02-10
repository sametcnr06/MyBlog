using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MyBlog.DataAccess.Abstract
{
    public interface IEntityRepository<T> where T : class, new()
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter); // Tek bir nesne getir.
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null); // Tüm nesneleri getir.
        Task AddAsync(T entity); // Yeni bir nesne ekle.
        Task UpdateAsync(T entity); // Bir nesneyi güncelle.
        Task DeleteAsync(T entity); // Bir nesneyi sil.
    }
}
