using DontForgetApp.Service;
using DontForgetApp.ViewModel;

namespace DontForgetApp.View;

public partial class HomeView : ContentPage
{
	public HomeViewModel ViewModel { get; set; }

	public HomeView(IReminderService reminderService)
	{
		InitializeComponent();
		ViewModel = new HomeViewModel(reminderService);
		BindingContext = ViewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		ViewModel.LoadReminderList();
	}
}