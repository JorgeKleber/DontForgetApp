using DontForgetApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontForgetApp.Service
{
	public interface INotifyService
	{
		Task CreateReminderNotification(Reminder reminder);
		Task UpdateReminderNotification(Reminder reminder);
		Task DeleteReminderNotification(int reminderId);
	}
}
