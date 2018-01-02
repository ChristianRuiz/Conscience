using Conscience.DataAccess.Repositories;
using Conscience.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Services
{
    public class NotificationsService
    {
        private readonly NotificationRepository _notificationsRepo;
        private readonly AccountRepository _accountRepo;

        public NotificationsService(NotificationRepository notificationsRepo, AccountRepository accountRepo)
        {
            _notificationsRepo = notificationsRepo;
            _accountRepo = accountRepo;
        }
        
        public bool HasUnprocessedNotifications(int accountId)
        {
            return _notificationsRepo.GetByAccount(accountId).Any(n => !n.Processed);
        }

        public Notification MarkAsRead(int notificationId)
        {
            var notification = _notificationsRepo.GetAll().First(n => n.Id == notificationId);
            notification.Read = true;
            _notificationsRepo.Modify(notification);
            return notification;
        }

        public void Notify(RoleTypes role, string text, NotificationTypes type, Host host = null, Employee employee = null, Audio audio = null)
        {
            var roleAccounts = _accountRepo.GetAll().Where(a => a.Roles.Any(r => r.Name == role.ToString())).Select(a => a.Id).ToList();
            foreach(var accountId in roleAccounts)
            {
                Notify(accountId, text, type, host, employee, audio);
            }
        }

        public void Notify(int accountId, string text, NotificationTypes type, Host host = null, Employee employee = null, Audio audio = null)
        {
            if (audio == null)
                audio = GetDefaultAdioByType(type);

            if (type == NotificationTypes.Reset)
                Reset(accountId);

            var notification = _notificationsRepo.GetAll().Where(n => n.OwnerId == accountId).OrderByDescending(n => n.Id).Take(2).FirstOrDefault(n => n.Description == text && n.NotificationType == type);

            if (notification == null)
            {
                _notificationsRepo.Add(new Notification
                {
                    Owner = _accountRepo.GetById(accountId),
                    Description = text,
                    NotificationType = type,
                    Host = host,
                    Employee = employee,
                    Audio = audio,
                    TimeStamp = DateTime.Now
                });
            }
            else
            {
                notification.Processed = false;
                notification.TimeStamp = DateTime.Now;
                if (type != NotificationTypes.StatsModified)
                    notification.Read = false;
                _notificationsRepo.Modify(notification);
            }
        }

        private void Reset(int accountId)
        {
            var account = _accountRepo.GetById(accountId);
            
            var notifications = _notificationsRepo.GetAll().Where(n => n.OwnerId == account.Id).ToList();
            foreach (var notification in notifications)
                _notificationsRepo.Delete(notification);

            _accountRepo.Modify(account);
        }

        private Audio GetDefaultAdioByType(NotificationTypes type)
        {
            Audio audio = null;

            switch(type)
            {
                case NotificationTypes.CallHost:
                    audio = new Audio { Transcription = "", Path = "/Content/audio/notifications/callhost.mp3" };
                    break;
                case NotificationTypes.LowBattery:
                    audio = new Audio { Transcription = "", Path = "/Content/audio/notifications/lowbattery.mp3" };
                    break;
                case NotificationTypes.Reset:
                    audio = new Audio { Transcription = "", Path = "/Content/audio/notifications/reset.mp3" };
                    break;
                case NotificationTypes.NoReset:
                    audio = new Audio { Transcription = "", Path = "/Content/audio/notifications/noreset.mp3" };
                    break;
                case NotificationTypes.SystemFailure:
                    audio = new Audio { Transcription = "", Path = "/Content/audio/notifications/systemfailure.mp3" };
                    break;
                case NotificationTypes.StatsModified:
                    audio = new Audio { Transcription = "", Path = "/Content/audio/notifications/statsmodified.mp3" };
                    break;
                case NotificationTypes.CharacterModified:
                    audio = new Audio { Transcription = "", Path = "/Content/audio/notifications/charactermodified.mp3" };
                    break;
                case NotificationTypes.CharacterAssigned:
                    audio = new Audio { Transcription = "", Path = "/Content/audio/notifications/characterassigned.mp3" };
                    break;
                case NotificationTypes.PlotModified:
                    audio = new Audio { Transcription = "", Path = "/Content/audio/notifications/plotmodified.mp3" };
                    break;
            }

            return audio;
        }
    }
}
