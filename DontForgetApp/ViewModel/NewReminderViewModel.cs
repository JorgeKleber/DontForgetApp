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
		public IReminderService reminderService { get; set; }

		public NewReminderViewModel(IReminderService service)
		{
			reminderService = service;

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

		private async void CreateReminderNotification()
		{
			try
			{
				var request = new NotificationRequest
				{
					NotificationId = 1337,
					Title = NewReminder.Title,
					Description = NewReminder.Description,
					BadgeNumber = 42,
					Schedule = new NotificationRequestSchedule
					{
						NotifyTime = NewReminder.RemindDateTime,
					}
				};

				await LocalNotificationCenter.Current.Show(request);
			}
			catch (Exception ex)
			{
				await Shell.Current.CurrentPage.DisplayAlert("Erro ao criar Lembrete", "Infelizmente houve um erro ao criar o lembrete", "Entendi");
			}
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

				int operationResult = await reminderService.AddReminder(NewReminder);

				if (operationResult == 1) 
				{
					CreateReminderNotification();
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