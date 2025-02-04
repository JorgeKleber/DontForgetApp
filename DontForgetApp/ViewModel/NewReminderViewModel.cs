using Android.Speech.Tts;
using CommunityToolkit.Mvvm.ComponentModel;
using DontForgetApp.Model;
using DontForgetApp.Service;
using Plugin.LocalNotification;
using System.Diagnostics;
using System.Windows.Input;

namespace DontForgetApp.ViewModel
{
	[QueryProperty(nameof(ReminderDateTime), "SelectedDate")]
	[QueryProperty(nameof(NewReminder), "NewReminder")]
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
		[ObservableProperty]
		private string _titlePage;

		private List<FileResult> _files;
		private bool isReminderUpdate;

		public ICommand RegisterOperation { get; set; }
		public ICommand DeleteReminder { get; set; }
		public ICommand AttachFile{ get; set; }

		public IDatabaseService BdService { get; set; }
		public INotifyService NotificationService { get; set; }

		public NewReminderViewModel(IDatabaseService dbService, INotifyService notifyService)
		{
			BdService = dbService;
			NotificationService = notifyService;

			AttachFile = new Command(AttachFileEvent);
			RegisterOperation = new Command(RegisterOperationEvent);
			DeleteReminder = new Command(DeleteReminderEvent);
		}

		public void Init()
		{
			if (NewReminder == null)
			{
				PlaceholderTitle = "Entry a Title here";
				PlaceholderDescription = "Entry the reminder description";

				TitlePage = "Novo Lembrete";
				NewReminder = new Reminder();
				NewReminder.Title = Title;
				NewReminder.Description = Description;
				NewReminder.RemindDateTime = ReminderDateTime;
				ReminderTime = DateTime.Now.TimeOfDay;
				isReminderUpdate = false;
			}
			else 
			{
				TitlePage = "Editar lembrete";
				Title = NewReminder.Title;
				Description = NewReminder.Description;
				ReminderDateTime = NewReminder.RemindDateTime;
				ReminderTime = NewReminder.RemindDateTime.TimeOfDay;
				isReminderUpdate = true;
			}
		}

		private bool CanSaveNewReminder()
		{
			if (string.IsNullOrEmpty(Title))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		private async void RegisterOperationEvent(object obj)
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
					if (!isReminderUpdate)
					{
						operationResult = await BdService.AddReminder(NewReminder);
					}
					else
					{
						operationResult = await BdService.UpdateReminder(NewReminder);
					}
				}
				else
				{
					if (!isReminderUpdate)
					{
						operationResult = await BdService.AddReminder(NewReminder, SelectedFiles);
					}
					else
					{
						operationResult = await BdService.UpdateReminder(NewReminder, SelectedFiles);
					}
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

		private async void DeleteReminderEvent(object obj)
		{
			if (!isReminderUpdate)
			{
				FinalizeOperation();
			}
			else
			{
				bool canDelete = await Shell.Current.DisplayAlert("Excluir Lembrete?", "Após excluir não será possível recuperar este lembrete, deseja realmente excluir?", "Sim", "Não");

				if (canDelete)
				{
					var operationStatus = BdService.DeleteReminder(NewReminder);

					if (operationStatus.Result != 1)
					{
						await Shell.Current.DisplayAlert("Ops!", "Parece que o lembrete não pôde ser excluído, tente repetir a operação ou reiniciar o aplicativo", "Entendi");
					}

					await NotificationService.DeleteReminderNotification(NewReminder.IdReminder);
				}

				FinalizeOperation();
			}
		}

		private async void FinalizeOperation()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}