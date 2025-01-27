using CommunityToolkit.Mvvm.ComponentModel;
using DontForgetApp.Model;
using DontForgetApp.Service;
using Plugin.LocalNotification;
using System.Windows.Input;

namespace DontForgetApp.ViewModel
{
	public partial class NewReminderViewModel : ObservableObject
	{
		[ObservableProperty]
		private string _placeholderTitle;
		[ObservableProperty]
		private string _placeholderDescription;
		[ObservableProperty]
		private DateTime _placeholderReminderDateTime;
		[ObservableProperty]
		private byte[] _fileAttached;
		[ObservableProperty]
		private Reminder _newReminder;
		[ObservableProperty]
		private TimeSpan _reminderTime;

		public ICommand SaveReminder { get; set; }
		public ICommand CancelReminder{ get; set; }
		public IDatabaseService BdService { get; set; }
		public INotifyService NotificationService { get; set; }

		public NewReminderViewModel(IDatabaseService dbService, INotifyService notifyService)
		{
			BdService = dbService;
			NotificationService = notifyService;

			CancelReminder = new Command(CancelReminderEvent);
			SaveReminder = new Command(SaveReminderEvent);

			Init();
		}

		private void Init()
		{
			PlaceholderTitle = "Entry a Title here";
			PlaceholderDescription = "Entry the reminder description";
			PlaceholderReminderDateTime = DateTime.Now;
			ReminderTime = PlaceholderReminderDateTime.TimeOfDay;

			NewReminder = new Reminder();
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
				NewReminder.RemindDateTime = PlaceholderReminderDateTime + ReminderTime;

				int operationResult = await BdService.AddReminder(NewReminder);

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

		private void CancelReminderEvent(object obj)
		{
			FinalizeOperation();
		}

		private async void FinalizeOperation()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}