using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Extensions;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EFurni.Infrastructure.Repositories
{
    internal class AccountRepository : IAccountRepository<AccountFilterParams>
    {
        private EFurniContext _dbContext;

        public AccountRepository(EFurniContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<Account>> GetAllAccountsAsync(AccountFilterParams filterParams = null, PaginationParams paginationParams = null)
        {
            var query = _dbContext.Account.Include(x => x.Customer).AsQueryable();

            query = AddFilterOnQuery(filterParams,query);
            
            if (paginationParams == null)
            {
                return await query.ToArrayAsync();
            }

            return await query.Skip(paginationParams.GetSkipAmount()).Take(paginationParams.PageSize).ToArrayAsync();
        }

        public async Task<bool> CreateAccountAsync(Account account)
        {
            await _dbContext.Account.AddAsync(account);
            
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Account> GetAccountByIdAsync(int accountId)
        {
            return await _dbContext.Account
                .Include(x => x.Customer)
                .Where(x => x.AccountId== accountId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAccountAsync(Account accountToUpdate)
        {
            _dbContext.Account.Update(accountToUpdate);
            
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = await GetAccountByIdAsync(accountId);
            
            if (account == null)
                return false;

            _dbContext.Account.Remove(account);

            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
        
        public IQueryable<Account> AddFilterOnQuery(AccountFilterParams filter, IQueryable<Account> query)
        {
            if (filter == null)
                return query;

            if (!string.IsNullOrEmpty(filter.AccountMail))
            {
                query = query.Where(f => f.Email == filter.AccountMail);
            }

            if (filter.AccountId != null)
            {
                query = query.Where(f => f.AccountId == filter.AccountId);
            }

            if (filter.AccountPassword != null)
            {
                query = query.Where(f => f.Password == filter.AccountPassword);
            }
            
            return query;
        }
        
    }
}