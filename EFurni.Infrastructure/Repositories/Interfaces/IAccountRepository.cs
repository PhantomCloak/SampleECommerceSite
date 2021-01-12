using System.Collections.Generic;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;

namespace EFurni.Infrastructure.Repositories
{
    public interface IAccountRepository <in TFilter> :
        IQueryFilter<TFilter, Account> where TFilter : class
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync(TFilter filterParams=null,PaginationParams paginationParams = null);
        Task<bool> CreateAccountAsync(Account account);
        Task<Account> GetAccountByIdAsync(int accountId);
        Task<bool> UpdateAccountAsync(Account accountToUpdate);
        Task<bool> DeleteAccountAsync(int accountId);
    }
}