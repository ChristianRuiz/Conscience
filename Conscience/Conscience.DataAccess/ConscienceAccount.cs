using Conscience.Domain;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concience.DataAccess
{
    public class ConscienceAccount : Account, IUser<int>
    {
    }
}
