using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Conscience.DataAccess.Repositories
{
    public class HostRepository : BaseRepository<Host>
    {
        public HostRepository(ConscienceContext context) : base(context)
        {
        }

        protected override IDbSet<Host> DbSet
        {
            get
            {
                return _context.Hosts;
            }
        }

        public IQueryable<Host> GetAllHosts(Account currentUser)
        {
            var hosts = GetAll().OfType<Host>();
            if (!currentUser.Roles.Any(r => r.Name == RoleTypes.Admin.ToString()))
                hosts = hosts.Where(h => !h.Hidden || (h.Hidden && h.HiddenHostAdministrators.Any(a => a.Account.Id == currentUser.Id)));
            return hosts;
        }

        public Host GetById(Account currentUser, int userId)
        {
            var host = GetAllHosts(currentUser).FirstOrDefault(u => u.Id == userId);
            return host;
        }

        public Host ModifyStats(int hostId, List<Stats> stats)
        {
            var host = DbSet.FirstOrDefault(u => u.Id == hostId);
            foreach (var stat in stats)
            {
                host.Stats.First(s => s.Name.ToLowerInvariant() == stat.Name.ToLowerInvariant()).Value = stat.Value;
            }
            Modify(host);
            return host;
        }

        public Host AssignHost(Host host, Character character)
        {
            if (character.CurrentHost != null)
            {
                character.CurrentHost.UnassignedOn = DateTime.Now;
            }

            var currentCharacter = host.CurrentCharacter;
            if (currentCharacter != null)
            {
                currentCharacter.UnassignedOn = DateTime.Now;
            }

            host.Characters.Add(new CharacterInHost
            {
                AssignedOn = DateTime.Now,
                Character = character
            });

            Modify(host);
            return host;
        }

        public Host ChangeStatus(int hostId, HostStatus status)
        {
            var host = GetAll().First(h => h.Id == hostId);
            host.Status = status;
            Modify(host);
            return host;
        }
    }
}
