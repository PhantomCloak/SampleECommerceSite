using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Repositories;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    internal class TokenService : ITokenService
    {
        private readonly IAccountRepository<AccountFilterParams> _accountRepository;
        private readonly ITokenRepository _tokenRepository;

        public TokenService(
            IAccountRepository<AccountFilterParams> accountRepository,
            ITokenRepository tokenRepository)
        {
            _accountRepository = accountRepository;
            _tokenRepository = tokenRepository;
        }

        public async Task<(bool Validated, Account ValidatedAccount)> ValidateTokenAsync(string token)
        {
            var account = await GetTokenAccountAsync(token);

            return (account == null, account);
        }

        public async Task<string> GetAccountTokenAsync(Account account)
        {
            return await _tokenRepository.GetTokenFromIdentifier(account.AccountId);
        }

        public async Task<Account> GetTokenAccountAsync(string token)
        {
            //get from redis
            var identifier = await _tokenRepository.GetIdentifierFromTokenAsync(token);

            var account = await _accountRepository.GetAccountByIdAsync(identifier);

            return account;
        }

        public async Task<string> CreateTokenAsync(Account account)
        {
            var token = await _tokenRepository.CreateTokenAsync(account.AccountId);

            return token;
        }

        public async Task DeleteTokenAsync(string token)
        {
            await _tokenRepository.DeleteTokenAsync(token);
        }
    }
}