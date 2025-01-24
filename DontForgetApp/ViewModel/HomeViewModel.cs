using CommunityToolkit.Mvvm.ComponentModel;
using DontForgetApp.Model;
using DontForgetApp.Service;
using System.Windows.Input;

namespace DontForgetApp.ViewModel
{
	public class HomeViewModel : ObservableObject
	{
		private IReminderService reminderService { get; set; }

		[ObservableProperty]
		private List<Reminder> _reminders;

		public ICommand AddNewReminder { get; set; }
		public ICommand UpdateReminder { get; set; }
		public ICommand DeleteReminder { get; set; }
		public ICommand ShowReminderDetails { get; set; }

		public HomeViewModel(IReminderService reminderService)
		{
			this.reminderService = reminderService;

			ShowReminderDetails = new Command(ShowReminderDetailsEvent);
			DeleteReminder = new Command(DeleteReminderEvent);
			UpdateReminder = new Command(UpdateReminderEvent);
			AddNewReminder = new Command(AddNewReminderEvent);
		}

		private void AddNewReminderEvent(object obj)
		{
			throw new NotImplementedException();
		}

		private void UpdateReminderEvent(object obj)
		{
			throw new NotImplementedException();
		}

		private void DeleteReminderEvent(object obj)
		{
			throw new NotImplementedException();
		}

		private void ShowReminderDetailsEvent(object obj)
		{
			throw new NotImplementedException();
		}
	}
}
