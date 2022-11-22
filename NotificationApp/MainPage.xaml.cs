using Microsoft.AspNetCore.SignalR.Client;
using Plugin.LocalNotification;

namespace NotificationApp;

public partial class MainPage : ContentPage
{
    private readonly HubConnection hubConnection;

    public MainPage()
    {
        InitializeComponent();

       // var baseUrl = "https://siasoftnoti.herokuapp.com";
        var baseUrl = "https://localhost";

        // Android can't connect to localhost
        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            baseUrl = "http://10.0.2.2";
            //baseUrl = "https://siasoftnoti.herokuapp.com";
        }

        hubConnection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}:5127/chatHub")
            .Build();

        hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            lblChat.Text += $"<b>{user}</b>: {message}<br/>";
            var request = new NotificationRequest
            {
                NotificationId = 1000, 
                Title = "Update v2",
                Subtitle = user,
                Description = message,
                BadgeNumber = 42,
                Sound = DeviceInfo.Platform == DevicePlatform.Android ? "<sound>" : "sound.mp3",
                Schedule = new NotificationRequestSchedule
                {
                    // NotifyTime = DateTime.Now.AddSeconds(1),
                    NotifyRepeatInterval = TimeSpan.FromDays(1)
                }
            };
            LocalNotificationCenter.Current.Show(request);
        });

        StartConection();

       /* Task.Run(() =>
        {
            Dispatcher.Dispatch(async () =>
            {
                await hubConnection.StartAsync();
                await hubConnection.InvokeAsync("AddToGroup", "software");
            });
        });*/
    }

    private async void StartConection()
    {
         await hubConnection.StartAsync();
        await hubConnection.InvokeAsync("AddToGroup", "software");
    }



    private async void btnSend_Clicked(object sender, EventArgs e)
    {
        await hubConnection.InvokeCoreAsync("UpdateScreen", args: new[]
        {
            picker.SelectedItem.ToString(),
            cost.Text
        }); ;

       /* await hubConnection.InvokeCoreAsync("SendMessageToAll", args: new[]
        {
                txtUsername.Text,
                txtMessage.Text,
                "software"
          });

        txtMessage.Text = String.Empty;*/
    }
}


