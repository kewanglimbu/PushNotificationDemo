using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Plugin.Firebase.CloudMessaging;

namespace PushNotificationDemo.ViewModels
{
    public partial class MainViewModel: ObservableObject
    {
        [ObservableProperty]
        private string _Token;
        [ObservableProperty]
        private FirebaseApp _App;

        public MainViewModel()
        {
            InitializeFirebaseApp();
        }

        private async void InitializeFirebaseApp()
        {
            if (App == null)
            {
                App = FirebaseApp.Create(new AppOptions
                {
                    Credential = await GetCredential()
                });
            }
        }

        [RelayCommand]
        private async Task OnGetTokenClicked()
        {
            await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
            var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
            Token = token;
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            await Toast.Make("Generate Fcm token Successfully",
                  ToastDuration.Long,
                  16)
            .Show(cancellationTokenSource.Token);
        }

        [RelayCommand]
        private async Task OnSendMessageClicked()
        {
            await Task.Delay(5000);
            FirebaseMessaging messaging = FirebaseMessaging.GetMessaging(App);
            var message = new Message()
            {
                Token = Token,
                Notification = new Notification { Title = "Free offer !!!", Body = "You can get 1 sneaker free while buying two sneaker.",ImageUrl= "https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/736cd272-24a0-4c03-b33d-9b73a5a47649/metcon-9-workout-shoes-ldMsxB.png" },
                Android = new AndroidConfig { Priority = Priority.High },
                Apns = new ApnsConfig { Headers = new Dictionary<string, string> { { "apns-priority", "5" } } }
            };
            var response = await messaging.SendAsync(message);
            await Shell.Current.DisplayAlert("Response", response, "OK");
        }

        private async Task<GoogleCredential> GetCredential()
        {
            try
            {
                var path = await FileSystem.OpenAppPackageFileAsync("pushnotification-maui-firebase-adminsdk-xi64j-f4d66f7b59.json");
                return GoogleCredential.FromStream(path);
            }
            catch (Exception ex) 
            {
                await Console.Out.WriteLineAsync("Something went wrong."+ ex);
                return null;
            }
        }
    }
}
