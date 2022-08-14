// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StalkerMUD.Client.Screens;
using StalkerMUD.Client.UI;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => {
        services.AddScoped<MainMenuScreen>();
        services.AddTransient<ScreenPlayer>(); 
    })
    .Build();

var screens = host.Services.GetRequiredService<ScreenPlayer>();
screens.StartShowCycle<MainMenuScreen>();