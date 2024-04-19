using PushNotificationDemo.ViewModels;

namespace PushNotificationDemo
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainViewModel mainViewModel)
        {
            InitializeComponent();
            BindingContext = mainViewModel;
        }
    }

}
