using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concience.DataAccess
{
    public class ConscienceContext : DbContext
    {
        public ConscienceContext() : base("ConscienceDB")
        {
        }

        public DbSet<ConscienceAccount> Accounts
        {
            get;
            set;
        }

        public DbSet<Role> Roles
        {
            get;
            set;
        }

        public DbSet<User> Users
        {
            get;
            set;
        }

        public DbSet<Device> Devices
        {
            get;
            set;
        }

        public DbSet<Location> Locations
        {
            get;
            set;
        }

        public DbSet<Character> Characters
        {
            get;
            set;
        }

        public DbSet<Memory> Memories
        {
            get;
            set;
        }

        public DbSet<Plot> Plots
        {
            get;
            set;
        }

        public DbSet<Notification> Notifications
        {
            get;
            set;
        }

        public DbSet<Audio> Audios
        {
            get;
            set;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Conscience");
            
            modelBuilder.Entity<Account>().HasMany(a => a.Roles).WithMany(r => r.Accounts);
            modelBuilder.Entity<User>().HasOptional(u => u.Device);
            modelBuilder.Entity<Device>().HasMany(d => d.Locations);
            modelBuilder.Entity<Host>().ToTable("Hosts");
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Employee>().HasRequired(e => e.Account).WithOptional(a => a.Employee);
            modelBuilder.Entity<Host>().HasRequired(e => e.Account).WithOptional(a => a.Host);
            modelBuilder.Entity<Host>().HasMany(h => h.Stats).WithRequired(s => s.Host);
            modelBuilder.Entity<Host>().HasMany(h => h.CoreMemories);
            modelBuilder.Entity<Host>().HasMany(h => h.Characters);
            modelBuilder.Entity<CoreMemory>().ToTable("CoreMemories");
            modelBuilder.Entity<CharacterInHost>().HasRequired(c => c.Character);
            modelBuilder.Entity<Character>().HasMany(c => c.Memories);
            modelBuilder.Entity<Character>().HasMany(c => c.Triggers);
            modelBuilder.Entity<Character>().HasMany(c => c.PlotEvents).WithMany(e => e.Characters);
            modelBuilder.Entity<Character>().HasMany(c => c.Plots);
            modelBuilder.Entity<CharacterInPlot>().HasRequired(c => c.Plot);
            modelBuilder.Entity<Plot>().HasMany(c => c.Events).WithRequired(e => e.Plot);
            modelBuilder.Entity<NotificationStatChange>().ToTable("NotificationsStatChange");
            modelBuilder.Entity<NotificationPlotChange>().ToTable("NotificationsPlotChange");
            modelBuilder.Entity<NotificationAudio>().ToTable("NotificationsAudio");
            modelBuilder.Entity<NotificationStatChange>().HasRequired(c => c.Stat);
            modelBuilder.Entity<NotificationPlotChange>().HasRequired(c => c.Plot);
            modelBuilder.Entity<NotificationPlotChange>().HasMany(c => c.Characters);
            modelBuilder.Entity<NotificationAudio>().HasRequired(c => c.Audio);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
