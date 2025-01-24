using DontForgetApp.Service;
using DontForgetApp.ViewModel;

namespace DontForgetApp.View;

public partial class HomeView : ContentPage
{
	public HomeView(IReminderService reminderService)
	{
		InitializeComponent();
		BindingContext = new HomeViewModel(reminderService);
	}
}