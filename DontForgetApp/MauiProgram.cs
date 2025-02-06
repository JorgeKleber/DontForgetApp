using CommunityToolkit.Maui;
using DontForgetApp.Service;
using DontForgetApp.View;
using DontForgetApp.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Plugin.LocalNotification;

namespace DontForgetApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if ANDROID
			EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
			{
				// remove o background padrão (underline) do Entry no Android
				handler.PlatformView.SetBackground(null);
			});
            DatePickerHandler.Mapper.AppendToMapping("NoUnderLine", (hander, view) => 
            {
                hander.PlatformView.SetBackground(null);
            });
			TimePickerHandler.Mapper.AppendToMapping("NoUnderLine", (hander, view) =>
			{
				hander.PlatformView.SetBackground(null);
			});
#endif

#if DEBUG
			builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<HomeView>();
            builder.Services.AddSingleton<HomeViewModel>();

            builder.Services.AddTransient<NewReminderView>();
            builder.Services.AddTransient<NewReminderViewModel>();

			builder.Services.AddTransientPopup<DetailPopupView, DetailPopupViewModel>();

			builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<INotifyService, NotifyService>();
            return builder.Build();
        }
    }
}
