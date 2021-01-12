using System.Linq;

namespace EFurni.Infrastructure
{
    public interface IQueryFilter<in TFilter, TEntity>
    {
        IQueryable<TEntity> AddFilterOnQuery(TFilter filter, IQueryable<TEntity> query);
    }
}