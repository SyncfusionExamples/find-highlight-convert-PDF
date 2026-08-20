using Microsoft.Extensions.Logging;

namespace Syncfusion_MauiWordtoPDFSample
{
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

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddTransient<Pages.WordToPdfPage>();
            builder.Services.AddTransient<PageModels.WordToPdfPageModel>();

            return builder.Build();
        }
    }
}
