using DontForgetApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace DontForgetApp.Service
{
	public class ReminderService : IReminderService
	{
		private SQLiteAsyncConnection _dbConnection;

		public async Task InitAsync()
		{
			if (_dbConnection == null)
			{
				try
				{
					string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Reminder.db3");

					_dbConnection = new SQLiteAsyncConnection(path);
					await _dbConnection.CreateTableAsync<Reminder>();
				}
				catch (Exception exception)
				{
					Debug.WriteLine("Ocorreu um erro na função InitAsync: " + exception.Message);
				}
			}
		}

		public Task<int> AddReminder(Reminder reminder)
		{
			try
			{
				return _dbConnection.InsertAsync(reminder);
				
			}
			catch (Exception exception)
			{
				Debug.WriteLine("Ocorreu um erro na função AddReminder: " + exception.Message);
				return Task.FromResult(-1);

			}
		}

		public Task<int> UpdateReminder(Reminder reminder)
		{
			try
			{
				return _dbConnection.UpdateAsync(reminder);
			}
			catch (Exception exception)
			{
				Debug.WriteLine("Ocorreu um erro na função UpdateReminder: " + exception.Message);
				return Task.FromResult(-1);
			}
		}

		public Task<int> DeleteReminder(Reminder reminder)
		{
			try
			{
				return _dbConnection.DeleteAsync(reminder);
			}
			catch (Exception exception)
			{
				Debug.WriteLine("Ocorreu um erro na função DeleteReminder: " + exception.Message);
				return Task.FromResult(-1);
			}
		}

		public Task<Reminder> GetReminderById(int id)
		{
			try
			{
				return _dbConnection.Table<Reminder>().FirstOrDefaultAsync(x => x.IdReminder == id);
			}
			catch (Exception exception)
			{
				Debug.WriteLine("Ocorreu um erro na função GetReminderById: " + exception.Message);
				return Task.FromResult(new Reminder());
			}
		}

		public Task<List<Reminder>> GetReminders()
		{
			try
			{
				return _dbConnection.Table<Reminder>().ToListAsync();
			}
			catch (Exception exception)
			{
				Debug.WriteLine("Ocorreu um erro na função GetReminders: " + exception.Message);
				return Task.FromResult(new List<Reminder>());
			}
		}
	}
}
