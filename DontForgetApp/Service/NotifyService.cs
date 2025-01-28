using DontForgetApp.Model;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontForgetApp.Service
{
	public class NotifyService : INotifyService
	{
		public async Task CreateReminderNotification(Reminder reminder)
		{
			try
			{
				var request = new NotificationRequest
				{
					NotificationId = reminder.IdReminder,
					Title = reminder.Title,
					Description = reminder.Description,
					BadgeNumber = 42,
					CategoryType = NotificationCategoryType.Reminder,
					Schedule = new NotificationRequestSchedule
					{
						NotifyTime = reminder.RemindDateTime,
					}
				};

				await LocalNotificationCenter.Current.Show(request);
			}
			catch (Exception ex)
			{
				await Shell.Current.CurrentPage.DisplayAlert("Erro ao criar Lembrete", "Infelizmente houve um erro ao criar o lembrete", "Entendi");
			}
		}

		public async Task DeleteReminderNotification(int reminderId)
		{
			try
			{
				LocalNotificationCenter.Current.Cancel(reminderId);
			}
			catch (Exception ex)
			{
				await Shell.Current.CurrentPage.DisplayAlert("Falha na operação", "Infelizmente houve uma falha ao deletar a notificação do lembrete.", "Entendi");
			}
		}

		public async Task UpdateReminderNotification(Reminder reminder)
		{
			try
			{
				await DeleteReminderNotification(reminder.IdReminder);

				var request = new NotificationRequest
				{
					NotificationId = reminder.IdReminder,
					Title = reminder.Title,
					Description = reminder.Description,
					BadgeNumber = 42,
					Schedule = new NotificationRequestSchedule
					{
						NotifyTime = reminder.RemindDateTime,
					}
				};

				await LocalNotificationCenter.Current.Show(request);
			}
			catch (Exception ex)
			{
				await Shell.Current.CurrentPage.DisplayAlert("Falha na operação", "Infelizmente houve uma falha ao atualizar a notificação do lembrete.", "Entendi");
			}
		}
	}
}
