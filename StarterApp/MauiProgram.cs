using Microsoft.Extensions.Logging;
using StarterApp.ViewModels;
using StarterApp.Database.Data;
using StarterApp.Views;
using System.Diagnostics;
using StarterApp.Services;
using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data.Repositories;


namespace StarterApp;

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

        builder.Services.AddDbContext<AppDbContext>();

        const bool useSharedApi = true;

        if (useSharedApi)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://set09102-api.b-davison.workers.dev/")
            };

            builder.Services.AddSingleton(httpClient);
            builder.Services.AddSingleton<IAuthenticationService, ApiAuthenticationService>();
            builder.Services.AddSingleton<IApiService, ApiService>();
            builder.Services.AddTransient<IItemRepository, ApiItemRepository>();
        }
        else
        {
            builder.Services.AddSingleton<IAuthenticationService, LocalAuthenticationService>();
        }

        builder.Services.AddSingleton<INavigationService, NavigationService>();
        builder.Services.AddTransient<IItemRepository, ItemRepository>();
        builder.Services.AddTransient<IRentalRepository, RentalRepository>();
        builder.Services.AddTransient<IReviewRepository, ReviewRepository>();
        builder.Services.AddTransient<IReviewService, ReviewService>();
        builder.Services.AddTransient<IRentalService, RentalService>();
        



        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<App>();

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<UserDetailViewModel>();
        builder.Services.AddSingleton<TempViewModel>();
        builder.Services.AddTransient<UserListViewModel>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<RegisterViewModel>();
        builder.Services.AddTransient<ItemsListViewModel>();
        builder.Services.AddTransient<CreateItemViewModel>();
        builder.Services.AddTransient<ItemDetailViewModel>();
        builder.Services.AddTransient<EditItemViewModel>();
        builder.Services.AddTransient<RequestRentalViewModel>();
        builder.Services.AddTransient<RentalsViewModel>();
        builder.Services.AddTransient<SubmitReviewViewModel>();

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<UserListPage>();
        builder.Services.AddTransient<UserDetailPage>();
        builder.Services.AddTransient<TempPage>();
        builder.Services.AddTransient<ItemsListPage>();
        builder.Services.AddTransient<CreateItemPage>();
        builder.Services.AddTransient<ItemDetailPage>();
        builder.Services.AddTransient<EditItemPage>();
        builder.Services.AddTransient<RequestRentalPage>();
        builder.Services.AddTransient<RentalsPage>();
        builder.Services.AddTransient<SubmitReviewPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}