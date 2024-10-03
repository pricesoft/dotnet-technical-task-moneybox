using System;
using Moq;
using NUnit.Framework;

namespace Moneybox.App.Test
{
    [TestFixture]
    public class AccountTest
    {
        private Mock<User> _subjectUser;

        [OneTimeSetUp]
        public void SetUp()
        {
            _subjectUser = new Mock<User>(MockBehavior.Strict);
        }

        [Test]
        public void BalanceDecreasesByX_OnDebit()
        {
            const decimal amountToDebit = 100m;

            var accountNumber = Guid.NewGuid();
            var account = new Account(accountNumber, _subjectUser.Object, 5000m);

            //act
            account.Debit(amountToDebit);

            //assert
            Assert.AreEqual(4900m, account.Balance);
        }

        [Test]
        public void BalanceIncreasesByX_OnCredit()
        {
            const decimal amountToCredit = 1000m;

            var accountNumber = Guid.NewGuid();
            var account = new Account(accountNumber, _subjectUser.Object, 5000m);

            //act
            account.Credit(amountToCredit);

            //assert
            Assert.AreEqual(6000m, account.Balance);
        }

        [Test]
        public void Validate_PayInLimitCantBeExceeded() /** limit set at 4000m **/
        {
            const decimal amountToCreditEachTime = 2000m;

            var accountNumber = Guid.NewGuid();
            var account = new Account(accountNumber, _subjectUser.Object, 1000m);

            account.Credit(amountToCreditEachTime); //limit @ 2000
            account.Credit(amountToCreditEachTime); //limit @ 4000

            //assert
            Assert.Throws<InvalidOperationException>(()=> account.Credit(amountToCreditEachTime));
        }

    }
}
