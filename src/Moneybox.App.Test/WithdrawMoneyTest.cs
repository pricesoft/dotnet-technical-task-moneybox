using Moneybox.App.DataAccess;
using Moneybox.App.Domain.Services;
using Moneybox.App.Features;
using Moq;
using NUnit.Framework;
using System;

namespace Moneybox.App.Test
{
    [TestFixture]
    public class WithdrawMoneyTest
    {
        private Mock<INotificationService> _notificationService;
        private Mock<IAccountRepository> _accountRepository;

        private Mock<User> _subjectUser;

        [OneTimeSetUp]
        public void Setup()
        {
            _subjectUser = new Mock<User>(MockBehavior.Strict);
            
            _notificationService = new Mock<INotificationService>();

            //Account with balance at 395m
            var subjectAccount = new Account(Guid.NewGuid(), _subjectUser.Object, 395m);

            _accountRepository = new Mock<IAccountRepository>();
            _accountRepository.Setup(x => x.GetAccountById(It.IsAny<Guid>())).Returns(subjectAccount);
        }

        [Test]
        public void Test_NotifyCustomerOfLowFund_IfBalanceLessThan200m()
        {
            var withdrawMoney = new WithdrawMoney(_accountRepository.Object, _notificationService.Object);

            withdrawMoney.Execute(Guid.NewGuid(), 200);

            _notificationService.Verify(x => x.NotifyFundsLow(It.IsAny<string>()), Times.Once);
        }
    }
}
