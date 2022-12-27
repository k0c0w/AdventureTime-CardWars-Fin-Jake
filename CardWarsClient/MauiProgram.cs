using CardWarsClient.ViewModels;
using Microsoft.Extensions.Logging;

namespace CardWarsClient;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("ComicFont.otf", "Comic");
                fonts.AddFont("Hangyaboly-Regular.ttf", "HangyabolyRegular");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton<GamePage>();


        return builder.Build();
    }
}
