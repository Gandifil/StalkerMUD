using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StalkerMUD.Client.Logic;
using StalkerMUD.Client.Screens;
using StalkerMUD.Client.Screens.Auth;
using StalkerMUD.Client.UI;
using StalkerMUD.Common;
using System.Net.Http.Headers;
using System.Text.Json;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => {
        var host = context.Configuration["Server:Host"];
        services.AddSingleton(_ => new HttpClient());
        services.AddScoped(_ =>
        {
            var client = new AuthClient(host, new HttpClient());
            client.JsonSerializerSettings.PropertyNameCaseInsensitive = true;
            return client;
        });
        services.AddScoped(services =>
        {
            var client = new ShopsClient(host, services.GetRequiredService<HttpClient>());
            client.JsonSerializerSettings.PropertyNameCaseInsensitive = true;
            return client;
        });
        services.AddScoped(services =>
        {
            var connectionState = services.GetRequiredService<ConnectionState>();
            var token = connectionState?.Token ?? throw new ArgumentNullException();
            var connection = new HubConnectionBuilder()
                .WithUrl($"{host}/fight", options =>
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
            var client = new PlayerClient(host, services.GetRequiredService<HttpClient>());
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