using System;

namespace Moneybox.App
{
    public class Account
    {
        public Account(Guid id, User user, decimal balance)
        {
            if (Guid.Empty == id)
                throw new InvalidOperationException("id");

            if (user == null)
                throw new InvalidOperationException("user");

            if (balance < 0)
                throw new ArgumentOutOfRangeException("balance");

            Id = id;
            User = user;
            Balance = balance;
        }

        public const decimal PayInLimit = 4000m;

        public Guid Id { get; private set; }

        public User User { get; private set; }

        public decimal Balance { get; set; }

        public decimal Withdrawn { get; set; }

        public decimal PaidIn { get; set; }

        public void Debit(decimal amount)
        {
            var payOutExpBalance = Balance - amount;

            if (payOutExpBalance < 0m)
            {
                throw new InvalidOperationException("Insufficient funds to make transfer");
            }

            Balance -= amount;
            Withdrawn -= amount;
        }

        public void Credit(decimal amount)
        {
            var payInExpTotal = PaidIn + amount;

            if (payInExpTotal > PayInLimit)
            {
                throw new InvalidOperationException("Account pay in limit reached");
            }

            Balance += amount;
            PaidIn += amount;
        }
    }
}
