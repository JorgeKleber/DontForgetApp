using DontForgetApp.Service;
using DontForgetApp.ViewModel;
using System.Xml.Linq;

namespace DontForgetApp.View;

[QueryProperty(nameof(SelectedDate), "SelectedDate")]
public partial class NewReminderView : ContentPage
{
	public DateTime SelectedDate { get; set; }
	public NewReminderViewModel ViewModel { get; set; }
	public NewReminderView(IDatabaseService reminderService, INotifyService notifyService)
	{
		InitializeComponent();
		ViewModel = new NewReminderViewModel(reminderService, notifyService);
		BindingContext = ViewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		ViewModel.Init(SelectedDate);
	}
}