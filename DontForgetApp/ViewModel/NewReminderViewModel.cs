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

			CancelReminder = new Command(CancelReminderEvent);
			SaveReminder = new Command(SaveReminderEvent);

			Init();
		}

		private void Init()
		{
			_placeholderTitle = "Entry a Title here";
			_placeholderDescription = "Entry the reminder description";
			_placeholderReminderDateTime = DateTime.Today;

			NewReminder = new Reminder();
			NewReminder.RemindDateTime = _placeholderReminderDateTime;
		}

		private bool CanSaveNewReminder()
		{
			if (string.IsNullOrEmpty(NewReminder.Title))
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
				await reminderService.InitAsync();

				int operationResult = await reminderService.AddReminder(NewReminder);

				if (operationResult == 1) 
				{
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