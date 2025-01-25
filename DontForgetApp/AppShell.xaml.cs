using DontForgetApp.View;

namespace DontForgetApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(NewReminderView), typeof(NewReminderView));

        }
    }
}
