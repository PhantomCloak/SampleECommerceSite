using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Shared.Models;
using Microsoft.Extensions.Configuration;

namespace EFurni.Infrastructure.Repositories
{
    public class CachedAccountRepository : IAccountRepository<AccountFilterParams>
    {
        private readonly IAccountRepository<AccountFilterParams> _accountRepository;
        private IDistributedCacheAdapter _distributedCache;
        private readonly TimeSpan _defaultTtl;

        public CachedAccountRepository(
            IAccountRepository<AccountFilterParams> accountRepository,
            IConfiguration configuration,
            IDistributedCacheAdapter distributedCache)
        {
            _accountRepository = accountRepository;
            _distributedCache = distributedCache;
            _defaultTtl = TimeSpan.FromMinutes(int.Parse(configuration["CacheProfiles:AccountTtl"]));
        }

        public Task<IEnumerable<Account>> GetAllAccountsAsync(
            AccountFilterParams filterParams = null,
            PaginationParams paginationParams = null) =>
            _accountRepository.GetAllAccountsAsync(filterParams, paginationParams);

        public async Task<bool> CreateAccountAsync(Account account) =>
            await _accountRepository.CreateAccountAsync(account);

        public async Task<Account> GetAccountByIdAsync(int accountId)
        {
            var cacheKey = $"account:{accountId}";
            var account = await _distributedCache.GetAsync<Account>(cacheKey);

            if (account == null)
            {
                account = await _accountRepository.GetAccountByIdAsync(accountId);

                await _distributedCache.SetAsync(cacheKey, account, _defaultTtl);
            }

            return account;
        }

        public async Task<bool> UpdateAccountAsync(Account accountToUpdate)
        {
            var cacheKey = $"account:{accountToUpdate.AccountId}";
            await _distributedCache.DeleteAsync(cacheKey);
            
            return await _accountRepository.UpdateAccountAsync(accountToUpdate);
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var cacheKey = $"account:{accountId}";
            await _distributedCache.DeleteAsync(cacheKey);

            return await _accountRepository.DeleteAccountAsync(accountId);
        }

        public IQueryable<Account> AddFilterOnQuery(AccountFilterParams filter, IQueryable<Account> query) =>
            _accountRepository.AddFilterOnQuery(filter, query);
    }
}