using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concience.DataAccess
{
    public class ConscienceDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ConscienceContext>
    {
        protected override void Seed(ConscienceContext context)
        {
        }
    }
}
