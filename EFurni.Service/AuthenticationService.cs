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
        private readonly IAccountRepository<AccountFilterParams> _accountRepository;
        private readonly ICustomerRepository<CustomerFilterParams> _customerRepository;
        private readonly ICryptographyService _cryptographyService;

        public AuthenticationService(
            IAccountRepository<AccountFilterParams> accountRepository,
            ICustomerRepository<CustomerFilterParams> customerRepository,
            ICryptographyService cryptographyService)
        {
            // _dbContext = dbContext;
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
            _cryptographyService = cryptographyService;
        }

        public async Task<(bool Validated,Account ValidatedUser)> ValidateUser(string userName, string password)
        {
            string hashedPassword = _cryptographyService.HashString(password, userName);

            var account = (await _accountRepository.GetAllAccountsAsync(new AccountFilterParams()
            {
                AccountMail = userName,
                AccountPassword = hashedPassword
            })).FirstOrDefault();

            return account == null ? (false, null) : (true,account);
        }
        public async Task<bool> RegisterUserAsync(RegisterUserParams registerQuery)
        {
            var newAccount = new Account
                {Email = registerQuery.Email, 
                    Password = _cryptographyService.HashString(registerQuery.Password,registerQuery.Email)};

            await _accountRepository.CreateAccountAsync(newAccount);

            bool result = await _customerRepository.CreateCustomerAsync(new Customer
            {
                Account = newAccount,
                FirstName = registerQuery.FirstName,
                LastName = registerQuery.LastName,
                ProfilePictureUrl = "https://media.discordapp.net/attachments/263722271556894720/796456288988037190/unknown.png",
                PhoneNumber = registerQuery.PhoneNumber,
            });

            return result;
        }
    }
}