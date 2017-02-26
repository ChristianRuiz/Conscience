using Concience.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Services
{
    public interface IUsersIdentityService
    {
        ConscienceAccount CurrentUser { get; }

        Task<ConscienceAccount> RegisterAsync(string username, string email, string password);

        Task<ConscienceAccount> LoginAsync(string username, string password);

        Task LogoffAsync();

        Task ChangePasswordAsync(int userId, string newPassword);
    }
}
