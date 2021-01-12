using System.Linq;

namespace EFurni.Infrastructure
{
    public interface IQuerySorter<in TSorter, TEntity>
    {
        IQueryable<TEntity> AddSortOnQuery(TSorter sorter, IQueryable<TEntity> query);
    }
}