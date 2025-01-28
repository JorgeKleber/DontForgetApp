using DontForgetApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontForgetApp.Service
{
	public interface IDatabaseService
	{
		Task InitAsync();
		Task<List<Reminder>>GetReminders();
		Task<Reminder> GetReminderById(int id);
		Task<int>AddReminder(Reminder reminder, AttachFile[] files = null);
		Task<int>DeleteReminder(Reminder reminder);
		Task<int>UpdateReminder(Reminder reminder);
		Task<List<FileResult>> PickerSomeFiles();
	}
}
