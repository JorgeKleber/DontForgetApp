using CommunityToolkit.Mvvm.ComponentModel;
using DontForgetApp.Model;
using DontForgetApp.Service;
using DontForgetApp.View;
using Plugin.LocalNotification;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace DontForgetApp.ViewModel
{
	public partial class HomeViewModel : ObservableObject
	{

		[ObservableProperty]
		private ObservableCollection<Reminder> _reminders;
		[ObservableProperty]
		private Reminder _reminderSelected;

		public ICommand AddNewReminder { get; set; }
		public ICommand UpdateReminder { get; set; }
		public ICommand DeleteReminder { get; set; }
		public ICommand ShowReminderDetails { get; set; }

		private IReminderService reminderService { get; set; }

		public HomeViewModel(IReminderService reminderService)
		{
			this.reminderService = reminderService;

			ShowReminderDetails = new Command(ShowReminderDetailsEvent);
			DeleteReminder = new Command(DeleteReminderEvent);
			UpdateReminder = new Command(UpdateReminderEvent);
			AddNewReminder = new Command(AddNewReminderEvent);

			CheckPermissions();
		}

		private async void CheckPermissions()
		{
			if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
			{
				await LocalNotificationCenter.Current.RequestNotificationPermission();
			}
		}

		public void LoadReminderList()
		{
			var reminderCollection = reminderService.GetReminders().Result;
			Reminders = new ObservableCollection<Reminder>(reminderCollection);
		}

		private async void AddNewReminderEvent(object obj)
		{
			await Shell.Current.GoToAsync(nameof(NewReminderView));
		}

		private void UpdateReminderEvent(object obj)
		{
			throw new NotImplementedException();
		}

		private async void DeleteReminderEvent(object obj)
		{
			bool canDelete = await Shell.Current.DisplayAlert("Excluir Lembrete?", "Após excluir não será possível recuperar este lembrete, deseja realmente excluir?", "Sim", "Não");

			if (canDelete)
			{
				var operationStatus = reminderService.DeleteReminder(ReminderSelected);

				if (operationStatus.Result != 1)
				{
					await Shell.Current.DisplayAlert("Ops!", "Parece que o lembrete não pôde ser excluído, tente repetir a operação ou reiniciar o aplicativo", "Entendi");
				}

				LoadReminderList();
			}
		}

		private void ShowReminderDetailsEvent(object obj)
		{
			throw new NotImplementedException();
		}
	}
}
