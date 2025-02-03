using CommunityToolkit.Mvvm.ComponentModel;
using DontForgetApp.Model;
using DontForgetApp.Service;
using Plugin.LocalNotification;
using System.Diagnostics;
using System.Windows.Input;

namespace DontForgetApp.ViewModel
{
	[QueryProperty(nameof(ReminderDateTime), "SelectedDate")]
	public partial class NewReminderViewModel : ObservableObject
	{
		[ObservableProperty]
		private string _title;
		[ObservableProperty]
		private string _description;
		[ObservableProperty]
		private string _placeholderTitle;
		[ObservableProperty]
		private string _placeholderDescription;
		[ObservableProperty]
		private DateTime _reminderDateTime;
		[ObservableProperty]
		private byte[] _fileAttached;
		[ObservableProperty]
		private Reminder _newReminder;
		[ObservableProperty]
		private AttachFile[] _selectedFiles;
		[ObservableProperty]
		private TimeSpan _reminderTime;

		private List<FileResult> _files;

		public ICommand SaveReminder { get; set; }
		public ICommand AttachFile{ get; set; }
		public IDatabaseService BdService { get; set; }
		public INotifyService NotificationService { get; set; }

		public NewReminderViewModel(IDatabaseService dbService, INotifyService notifyService)
		{
			BdService = dbService;
			NotificationService = notifyService;

			AttachFile = new Command(AttachFileEvent);
			SaveReminder = new Command(SaveReminderEvent);

			Init();
		}

		private void Init()
		{
			PlaceholderTitle = "Entry a Title here";
			PlaceholderDescription = "Entry the reminder description";
			ReminderTime = DateTime.Now.TimeOfDay;

			NewReminder = new Reminder();
			NewReminder.Title = Title;
			NewReminder.Description = Description;
			NewReminder.RemindDateTime = ReminderDateTime;
		}

		private bool CanSaveNewReminder()
		{
			if (string.IsNullOrEmpty(PlaceholderTitle))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		private async void SaveReminderEvent(object obj)
		{
			var canSave = CanSaveNewReminder();

			if (canSave)
			{
				NewReminder.Title = Title;
				NewReminder.Description = string.IsNullOrEmpty(Description) ? string.Empty : Description;

				NewReminder.RemindDateTime = ReminderDateTime.Add(ReminderTime);

				int operationResult;

				if (SelectedFiles == null || SelectedFiles.Count() == 0)
				{
					operationResult = await BdService.AddReminder(NewReminder);
				}
				else
				{
					operationResult = await BdService.AddReminder(NewReminder, SelectedFiles);
				}

				if (operationResult == 1) 
				{
					await NotificationService.CreateReminderNotification(NewReminder);
					FinalizeOperation();
				}
				else
				{
					await Shell.Current.CurrentPage.DisplayAlert("Erro ao criar Lembrete", "Infelizmente houve um erro ao criar o lembrete", "Entendi");
				}

			}
			else
			{
				await Shell.Current.CurrentPage.DisplayAlert("Ops!", "O campo de título não pode ficar em branco", "Entendi");
			}
		}

		private async void AttachFileEvent(object obj)
		{
			var searchResult = await BdService.PickerSomeFiles();

			if (searchResult == null) 
				return;

			_files = searchResult;

			var listFiles = await ConvertFileResultToAttachFile(_files);

			SelectedFiles = listFiles.ToArray();
		}

		public async Task<byte[]> ConvertFileToBytes(string filePath)
		{
			try
			{
				return await File.ReadAllBytesAsync(filePath);
			}
			catch (Exception exception)
			{
				Debug.WriteLine("Ocorreu um erro na função ConvertFileToBytes: " + exception.Message);
				return null;
			}
		}

		private async Task<List<AttachFile>> ConvertFileResultToAttachFile(List<FileResult> files)
		{
			try
			{
				List<AttachFile> myAttachFiles = new List<AttachFile>();

				foreach (var file in files)
				{
					var newAttachFile = new AttachFile()
					{
						FileName = file.FileName,
						IdReminder = NewReminder.IdReminder,
						FileBytes = await ConvertFileToBytes(file.FullPath)
					};

					myAttachFiles.Add(newAttachFile);
				}

				return myAttachFiles;
			}
			catch (Exception exception)
			{
				Debug.WriteLine("Ocorreu um erro na função ConvertFileResultToAttachFile: " + exception.Message);
				return null;
			}
		}

		private async void FinalizeOperation()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}