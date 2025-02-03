using DontForgetApp.Service;
using DontForgetApp.ViewModel;
using System.Xml.Linq;

namespace DontForgetApp.View;
public partial class NewReminderView : ContentPage
{
	public NewReminderView(IDatabaseService reminderService, INotifyService notifyService)
	{
		InitializeComponent();
		BindingContext = new NewReminderViewModel(reminderService, notifyService);
	}
}