using CommunityToolkit.Maui.Core;
using DontForgetApp.Service;
using DontForgetApp.ViewModel;

namespace DontForgetApp.View;

public partial class HomeView : ContentPage
{
	public HomeViewModel ViewModel { get; set; }

	public HomeView(IDatabaseService reminderService, INotifyService notifyService, IPopupService popupService)
	{
		InitializeComponent();
		ViewModel = new HomeViewModel(reminderService, notifyService, popupService);
		BindingContext = ViewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		ViewModel.LoadReminderList();
	}
}