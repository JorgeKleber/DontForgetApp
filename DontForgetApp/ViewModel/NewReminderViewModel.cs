using CommunityToolkit.Mvvm.ComponentModel;
using DontForgetApp.Model;
using DontForgetApp.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

		public ICommand SaveReminder { get; set; }
		public ICommand CancelReminder{ get; set; }
		public IReminderService reminderService { get; set; }

		public NewReminderViewModel(IReminderService service)
		{
			reminderService = service;
			NewReminder = new Reminder();

			CancelReminder = new Command(CancelReminderEvent);
			SaveReminder = new Command(SaveReminderEvent);

			Init();
		}

		private void Init()
		{
			_placeholderTitle = "Entry a Title here";
			_placeholderDescription = "Entry the reminder description";
			_placeholderReminderDateTime = DateTime.Today;
			NewReminder.RemindDateTime = _placeholderReminderDateTime;
		}

		private async void SaveReminderEvent(object obj)
		{
			var teste = _newReminder;

			//await reminderService.InitAsync();
			//await reminderService.InitAsync();
		}

		private void CancelReminderEvent(object obj)
		{
			
		}
	}
}