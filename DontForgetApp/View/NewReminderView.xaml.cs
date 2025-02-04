using DontForgetApp.Service;
using DontForgetApp.ViewModel;
using System.Xml.Linq;

namespace DontForgetApp.View;
public partial class NewReminderView : ContentPage
{
	public NewReminderViewModel ViewModel { get; set; }

	public NewReminderView(IDatabaseService reminderService, INotifyService notifyService)
	{
		InitializeComponent();

		ViewModel = new NewReminderViewModel(reminderService, notifyService);
		BindingContext = ViewModel; 
	}

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
	{
		base.OnNavigatedTo(args);

		ViewModel.Init();
	}
}