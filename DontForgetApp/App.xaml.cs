﻿using DontForgetApp.Service;
using DontForgetApp.View;

namespace DontForgetApp
{
    public partial class App : Application
    {
        public App(IReminderService reminderService)
        {
            reminderService.InitAsync();
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}