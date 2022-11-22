using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace NotificationApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()



			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder
			.UseLocalNotification(config =>
        {
            config.AddAndroid(android =>
            {
                android.AddChannel(new NotificationChannelRequest
                {
                    Sound = "sound"
                });
            });
        });

        return builder.Build();
	}
}
