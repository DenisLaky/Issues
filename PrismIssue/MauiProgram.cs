using Microsoft.Extensions.Logging;

namespace PrismIssue;

public static class MauiProgram
{
    public static void ConfigurePrism(PrismAppBuilder builder)
    {
        _ = builder.RegisterTypes(RegisterNavigation)
            .AddGlobalNavigationObserver(context => context.Subscribe(navigationContext =>
            {
                if (navigationContext.Type == NavigationRequestType.Navigate)
                {
                    Console.WriteLine($"Navigation: {navigationContext.Uri}");
                }
                else
                {
                    Console.WriteLine($"Navigation: {navigationContext.Type}");
                }

                if (!navigationContext.Result.Success)
                {
                    Console.WriteLine(navigationContext.Result.Exception);

                    if (navigationContext.Result.Exception.InnerException != null)
                    {
                        Console.WriteLine(navigationContext.Result.Exception.InnerException);
                    }
                }
            }))
            .ConfigureLogging(logging => logging.AddDebug())
            .OnInitialized(OnInitialized)
            .OnAppStart(SetupStartupPage());
    }

    private static Func<INavigationService, Task> SetupStartupPage()
        => navigationService => navigationService.NavigateAsync(nameof(MainPage));

    private static void RegisterNavigation(IContainerRegistry containerRegistry)
    {
        containerRegistry.RegisterGlobalNavigationObserver();

        _ = containerRegistry.RegisterForNavigation<MainPage>(nameof(MainPage));
        _ = containerRegistry.RegisterForNavigation<SecondPage>(nameof(SecondPage));

    }


    private static void OnInitialized(IContainerProvider container)
    {
    }

    public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UsePrism(ConfigurePrism)
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}

