using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StalkerMUD.Client.Logic;
using StalkerMUD.Client.Screens;
using StalkerMUD.Client.Screens.Auth;
using StalkerMUD.Client.UI;
using StalkerMUD.Common;
using System.Net.Http.Headers;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => {
        services.AddScoped(_ =>
        {
            var client = new AuthClient(context.Configuration["Server:Host"], new HttpClient());
            client.JsonSerializerSettings.PropertyNameCaseInsensitive = true;
            return client;
        });
        services.AddScoped(services =>
        {
            var connectionState = services.GetRequiredService<ConnectionState>();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                connectionState?.Token ?? throw new ArgumentNullException());
            var client = new ShopsClient(context.Configuration["Server:Host"], httpClient);
            client.JsonSerializerSettings.PropertyNameCaseInsensitive = true;
            return client;
        });
        services.AddScoped(services =>
        {
            var connectionState = services.GetRequiredService<ConnectionState>();
            var token = connectionState?.Token ?? throw new ArgumentNullException();
            var connection = new HubConnectionBuilder()
                .WithUrl($"{context.Configuration["Server:Host"]}/fight", options =>
                {
                    options.AccessTokenProvider = async() => token;
                })
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            return connection;
        });
        services.AddScoped<IPlayerClient>(services =>
        {
            var connectionState = services.GetRequiredService<ConnectionState>();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                connectionState?.Token ?? throw new ArgumentNullException());
            var client = new PlayerClient(context.Configuration["Server:Host"], httpClient);
            client.JsonSerializerSettings.PropertyNameCaseInsensitive = true;
            return client;
        });

        // screens
        services.AddScoped<MainMenuScreen>();
        services.AddScoped<RegistrationScreen>();
        services.AddScoped<LoginScreen>();
        services.AddScoped<City>();
        services.AddScoped<ShopScreen>();
        services.AddScoped<CharacterView>();
        services.AddScoped<UpgradeCharacter>();
        services.AddScoped<FightScreen>();

        // singletone
        services.AddSingleton<ScreenPlayer>();
        services.AddSingleton<ConnectionState>();
    })
    .Build();

var screens = host.Services.GetRequiredService<ScreenPlayer>();

screens.AddNextScreen<MainMenuScreen>();
screens.StartShowCycle();