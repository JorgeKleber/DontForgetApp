﻿using DontForgetApp.Model;
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
	public class DatabaseService : IDatabaseService
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
					await _dbConnection.CreateTableAsync<AttachFile>();
				}
				catch (Exception exception)
				{
					Debug.WriteLine("Ocorreu um erro na função InitAsync: " + exception.Message);
				}
			}
		}

		public async Task<List<FileResult>> PickerSomeFiles()
		{
			try
			{
				var result = await FilePicker.Default.PickMultipleAsync();

				return result.ToList();
			}
			catch (Exception exception)
			{
				Debug.WriteLine("Ocorreu um erro na função PickerSomeFiles: " + exception.Message);
				return null;
			}
		}

		public Task<int> AddReminder(Reminder reminder, AttachFile[] files = null)
		{
			try
			{
				var result = _dbConnection.InsertAsync(reminder);

				if (files != null)
					AddAttach(files);

				return result;
				
			}
			catch (Exception exception)
			{
				Debug.WriteLine("Ocorreu um erro na função AddReminder: " + exception.Message);
				return Task.FromResult(-1);
			}
		}

		private Task AddAttach(AttachFile[] files)
		{
			try
			{
				var result = _dbConnection.InsertAllAsync(files);

					return result;

			}
			catch (Exception exception)
			{
				Debug.WriteLine("Ocorreu um erro na função AddAttach: " + exception.Message);
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
				var files = _dbConnection.Table<AttachFile>().Where(x => x.IdReminder == reminder.IdReminder).ToListAsync();

				if (files.Result != null || files.Result?.Count != 0)
				{
					foreach (AttachFile item in files.Result)
					{
						_dbConnection.DeleteAsync(item);
					} 
				}

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
