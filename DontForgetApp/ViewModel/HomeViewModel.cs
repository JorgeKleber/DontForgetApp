using Android.App.Usage;
using CommunityToolkit.Mvvm.ComponentModel;
using DontForgetApp.Enuns;
using DontForgetApp.Model;
using DontForgetApp.Service;
using DontForgetApp.View;
using Plugin.LocalNotification;
using Plugin.Maui.Calendar.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace DontForgetApp.ViewModel
{
	public partial class HomeViewModel : ObservableObject
	{

		[ObservableProperty]
		private ObservableCollection<Reminder> _reminders;
		[ObservableProperty]
		private Reminder _reminderSelected;
		[ObservableProperty]
		public EventCollection _eventsDates;
		[ObservableProperty]
		public DateTime _selectedDate;

		public FilterType TypeFiter { get; set; }


		public ICommand AddNewReminder { get; set; }
		public ICommand UpdateReminder { get; set; }
		public ICommand DeleteReminder { get; set; }
		public ICommand ShowReminderDetails { get; set; }
		public ICommand FilterSearch { get; set; }
		public ICommand SearchTextChange { get; set; }

		private IDatabaseService DbService { get; set; }
		private INotifyService NotificationService { get; set; }


		public HomeViewModel(IDatabaseService reminderService, INotifyService notificationService)
		{
			this.DbService = reminderService;
			NotificationService = notificationService;
			SelectedDate = DateTime.Now.Date;

			ShowReminderDetails = new Command(ShowReminderDetailsEvent);
			SearchTextChange = new Command<string>(SearchTextChangeEvent);
			DeleteReminder = new Command(DeleteReminderEvent);
			UpdateReminder = new Command(UpdateReminderEvent);
			AddNewReminder = new Command(AddNewReminderEvent);
			FilterSearch = new Command(FilterSearchEvent);


			Reminders = new ObservableCollection<Reminder>();
			EventsDates = new EventCollection();
		}

		private void SearchTextChangeEvent(string searchText)
		{

			if (searchText.Length < 3)
				return;

			IEnumerable<EventModel> dates;

			if (string.IsNullOrEmpty(searchText))
			{
				dates = Reminders
						.GroupBy(e => e.RemindDateTime.Date)
						.Select(group => new EventModel
						{
							DateEvent = group.Key,
							RemindersList = group.ToList()
						});
			}
			else if (TypeFiter == FilterType.Title)
			{
				dates = Reminders
						.Where(e => e.Title.ToUpper() == searchText.ToUpper())
						.GroupBy(e => e.RemindDateTime.Date)
						.Select(group => new EventModel
						{
							DateEvent = group.Key,
							RemindersList = group.ToList()
						});
			}
			else
			{
				dates = Reminders
						.Where(e => e.Description.ToUpper().Contains(searchText.ToUpper()))
						.GroupBy(e => e.RemindDateTime.Date)
						.Select(group => new EventModel
						{
							DateEvent = group.Key,
							RemindersList = group.ToList()
						});
			}


			foreach (var date in dates)
			{
				if (!EventsDates.ContainsKey(date.DateEvent))
				{
					EventsDates.Add(date.DateEvent, date.RemindersList);
				}
				else
				{
					EventsDates[date.DateEvent] = date.RemindersList;
				}
			}


		}

		private async void FilterSearchEvent(object obj)
		{
			string action = await Shell.Current.DisplayActionSheet("Busca lembretes por: ", "Cancel", null, "Título", "Descrição");

			switch (action)
			{
				case "Título":

					TypeFiter = FilterType.Title;

				break;

				case "Descrição":

					TypeFiter = FilterType.Description;

					break ;
				default:

					TypeFiter = FilterType.Title;

					break;
			}
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
			CheckPermissions();

			var reminderCollection = DbService.GetReminders().Result;
			Reminders = new ObservableCollection<Reminder>(reminderCollection);


			var dates = Reminders
						.GroupBy(e => e.RemindDateTime.Date)
						.Select(group => new EventModel
						{
							DateEvent = group.Key,
							RemindersList = group.ToList()
						});

			foreach (var date in dates)
			{
				if (!EventsDates.ContainsKey(date.DateEvent))
				{
					EventsDates.Add(date.DateEvent, date.RemindersList);
				}
				else
				{
					EventsDates[date.DateEvent] = date.RemindersList;
				}
			}
		}

		private async void AddNewReminderEvent(object obj)
		{
			var selectedDate = new Dictionary<string, object>{
				{ "SelectedDate", SelectedDate}
			};

			await Shell.Current.GoToAsync(nameof(NewReminderView),true, selectedDate);
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
				var operationStatus = DbService.DeleteReminder(ReminderSelected);

				if (operationStatus.Result != 1)
				{
					await Shell.Current.DisplayAlert("Ops!", "Parece que o lembrete não pôde ser excluído, tente repetir a operação ou reiniciar o aplicativo", "Entendi");
				}

				await NotificationService.DeleteReminderNotification(ReminderSelected.IdReminder);

				LoadReminderList();
			}
		}

		private async void ShowReminderDetailsEvent(object obj)
		{
			await Shell.Current.GoToAsync(nameof(NewReminderView));
		}
	}
}
