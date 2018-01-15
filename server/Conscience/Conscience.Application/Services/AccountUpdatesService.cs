using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Services
{
    public class AccountUpdatesService
    {
        public static event EventHandler<AccountUpdatedEventArgs> AccountUpdated;

        public void BroadcastAccountUpdated(int accountId)
        {
            if (AccountUpdated != null)
                AccountUpdated(this, new AccountUpdatedEventArgs(accountId));
        }
    }

    public class AccountUpdatedEventArgs : EventArgs
    {
        public AccountUpdatedEventArgs(int accountId)
        {
            AccountId = accountId;
        }

        public int AccountId
        {
            get;
            set;
        }
    }
}
