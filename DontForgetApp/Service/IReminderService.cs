using DontForgetApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontForgetApp.Service
{
	public interface IReminderService
	{
		Task InitAsync();
		Task<List<Reminder>>GetReminders(DateTime date);
		Task<Reminder> GetReminderById(int id);
		Task<int>AddReminder(Reminder reminder);
		Task<int>DeleteReminder(int id);
		Task<int>UpdateReminder(Reminder reminder);
	}
}
