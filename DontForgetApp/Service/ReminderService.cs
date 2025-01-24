using DontForgetApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontForgetApp.Service
{
	public class ReminderService : IReminderService
	{
		private SQLiteAsyncConnection _dbConnection;

		public async Task InitAsync()
		{
			if (_dbConnection == null)
			{
				string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Reminder.db3");

				_dbConnection = new SQLiteAsyncConnection(path);
				await _dbConnection.CreateTableAsync<Reminder>();
			}
		}

		public Task<int> AddReminder(Reminder reminder)
		{
			return _dbConnection.InsertAsync(reminder);
		}

		public Task<int> UpdateReminder(Reminder reminder)
		{
			return _dbConnection.UpdateAsync(reminder);
		}

		public Task<int> DeleteReminder(int id)
		{
			return _dbConnection.DeleteAsync(id);
		}

		public Task<Reminder> GetReminderById(int id)
		{
			return _dbConnection.Table<Reminder>().FirstOrDefaultAsync(x => x.IdReminder == id);
		}

		public Task<List<Reminder>> GetReminders(DateTime date)
		{
			return _dbConnection.Table<Reminder>().ToListAsync();
		}
	}
}
