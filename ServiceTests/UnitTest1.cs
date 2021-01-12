using System;
using System.Threading.Tasks;
using Bogus;
using EFurni.Contract.V1.Queries.QueryParams;
using EFurni.Infrastructure.Repositories;
using EFurni.Shared.Models;
using Moq;
using NUnit.Framework;
using ServiceTests.Extensions;

namespace ServiceTests
{
    public class Tests
    {
        private Mock<IAccountRepository<AccountFilterParams>> _mockAccountRepository;
        
        [OneTimeSetUp]
        public void Setup()
        {
            _mockAccountRepository = new Mock<IAccountRepository<AccountFilterParams>>(MockBehavior.Strict);
            
            _mockAccountRepository
                .Setup(arg => arg.GetAccountByIdAsync(1))
                .ReturnsAsync(FakeEntity.FakeAccounts(10).Random());
        }

        [Test]
        public async Task Test1()
        {
            var account1 = await _mockAccountRepository.Object.GetAccountByIdAsync(1);
            var account2 = await _mockAccountRepository.Object.GetAccountByIdAsync(2);

            Console.WriteLine(account1.AccountId);
            Console.WriteLine(account2.AccountId);
            
            Assert.AreSame(account1,account2);
        }
    }
}