using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using DontForgetApp.Model;
using DontForgetApp.Service;
using DontForgetApp.View;
using System.Windows.Input;

namespace DontForgetApp.ViewModel
{
	public partial class DetailPopupViewModel : ObservableObject
	{

		[ObservableProperty]
		private Reminder _reminderSelected;

		private readonly IPopupService popupService;
		public Action DeleteDelegate;
		public IDatabaseService DbService { get; set; }
		public INotifyService NotificationService { get; set; }

		public ICommand DeleteReminder { get; set; }
		public ICommand EditReminder { get; set; }
		public ICommand ClosePopup { get; set; }



		public DetailPopupViewModel(IPopupService popupService)
		{
			this.popupService = popupService;

			EditReminder = new Command(EditReminderEvent);
			DeleteReminder = new Command(DeleteReminderEvent);
			ClosePopup = new Command(ClosePopupEvent);
		}

		private void ClosePopupEvent(object obj)
		{
			popupService.ClosePopup();
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
			}

			this.popupService.ClosePopup();
		}

		private async void EditReminderEvent(object obj)
		{
			var selectedDate = new Dictionary<string, object>{
				{ "NewReminder", ReminderSelected}
			};

			await this.popupService.ClosePopupAsync();

			await Shell.Current.GoToAsync($"{nameof(NewReminderView)}", true, selectedDate);
		}
	}
}