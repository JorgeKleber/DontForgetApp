using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using DontForgetApp.ViewModel;
using DontForgetApp.Service;

namespace DontForgetApp.View;

public partial class DetailPopupView : Popup
{
	public DetailPopupView(IPopupService popupService)
	{
		InitializeComponent();
		BindingContext = new DetailPopupViewModel(popupService);
	}
}