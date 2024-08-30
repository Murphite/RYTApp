using RYT.Data;

namespace RYT.Services.Repositories
{
    public interface IRepository
    {
        public RYTDbContext _ctx { get; }
        Task<int> AddAsync<T>(T entity) where T : class;
        Task<int> UpdateAsync<T>(T entity) where T : class;
        Task<int> DeleteAsync<T>(T entity) where T : class;
        Task<IQueryable<T>> GetAsync<T>() where T : class;
        Task<T?> GetAsync<T>(string Id) where T : class;
    }
}

