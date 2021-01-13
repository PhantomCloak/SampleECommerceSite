#pragma warning disable 8619
using System.Linq;
using System.Threading.Tasks;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Repositories;
using EFurni.Shared.Models;

namespace EFurni.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IAccountRepository<AccountFilterParams> _accountRepository;
        private readonly ICustomerRepository<CustomerFilterParams> _customerRepository;
        private readonly ICryptographyService _cryptographyService;

        public AuthenticationService(
            IAccountRepository<AccountFilterParams> accountRepository,
            ICustomerRepository<CustomerFilterParams> customerRepository,
            ITokenRepository tokenRepository,
            ICryptographyService cryptographyService)
        {
            _tokenRepository = tokenRepository;
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _cryptographyService = cryptographyService;
        }

        public async Task<string> LoginAsync(string userName, string password)
        {
            string hashedPassword = _cryptographyService.HashString(password, userName);

            var account = (await _accountRepository.GetAllAccountsAsync(new AccountFilterParams()
            {
                AccountMail = userName,
                AccountPassword = hashedPassword
            })).FirstOrDefault();

            if (account == null)
            {
                return null;
            }
            
            var token = await _tokenRepository.CreateTokenAsync(account.AccountId);

            return token;
        }

        public async Task<bool> RegisterUserAsync(RegisterUserParams registerQuery)
        {
            var newAccount = new Account
            {
                Email = registerQuery.Email,
                Password = _cryptographyService.HashString(registerQuery.Password, registerQuery.Email)
            };

            await _accountRepository.CreateAccountAsync(newAccount);

            bool result = await _customerRepository.CreateCustomerAsync(new Customer
            {
                Account = newAccount,
                FirstName = registerQuery.FirstName,
                LastName = registerQuery.LastName,
                ProfilePictureUrl =
                    "https://e7.pngegg.com/pngimages/109/994/png-clipart-teacher-student-college-school-education-avatars-child-face-thumbnail.png",
                PhoneNumber = registerQuery.PhoneNumber,
            });

            return result;
        }

        public async Task<bool> AuthenticateUser(string token)
        {
            var actorId = await GetTokenActorIdAsync(token);

            if (actorId == null || actorId <= 0 )
                return false;

            return true;
        }

        public async Task<int> GetTokenActorIdAsync(string token)
        {
            var actorId = await _tokenRepository.ActorIdFromToken(token);

            if (actorId == null)
                return 0;
            
            return int.Parse(actorId);
        }
    }
}