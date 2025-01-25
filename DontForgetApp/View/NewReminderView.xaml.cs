using DontForgetApp.Service;
using DontForgetApp.ViewModel;

namespace DontForgetApp.View;

public partial class NewReminderView : ContentPage
{
	public NewReminderView(IReminderService reminderService)
	{
		InitializeComponent();
		BindingContext = new NewReminderViewModel(reminderService);
	}
}