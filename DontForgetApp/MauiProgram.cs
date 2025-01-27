﻿using DontForgetApp.Service;
using DontForgetApp.View;
using DontForgetApp.ViewModel;
using Microsoft.Extensions.Logging;
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
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<HomeView>();
            builder.Services.AddSingleton<HomeViewModel>();

            builder.Services.AddTransient<NewReminderView>();
            builder.Services.AddTransient<NewReminderViewModel>();

            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<INotifyService, NotifyService>();
            return builder.Build();
        }
    }
}
