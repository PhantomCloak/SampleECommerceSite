using System;
using System.Collections.Generic;
using Bogus;
using EFurni.Shared.Models;
using ServiceTests.CryptoFunctions;

namespace ServiceTests
{
    public static class FakeEntity
    {
        public static IEnumerable<Account> FakeAccounts(int number)
        {
            int customerCounter = 1;
            var fakeAccounts = new Faker<Account>()
                .RuleFor(a => a.AccountId, f => customerCounter++)
                .RuleFor(a => a.Email, f => f.Person.Email)
                .RuleFor(a => a.Password, CryptoHelper.CreateMd5(CryptoHelper.RandomString(16)))
                .RuleFor(a=>a.DeletedAt,f=> DateTime.Now)
                .RuleFor(a=>a.Deleted,f=> false);

            return fakeAccounts.Generate(number);
        }
    }
}