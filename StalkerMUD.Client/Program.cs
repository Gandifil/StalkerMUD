// See https://aka.ms/new-console-template for more information
using StalkerMUD.Client.Screens;
using StalkerMUD.Client.UI;

Screen.StartShowCycle(new City(new StalkerMUD.Client.Game()));
//MainMenuShow();
 
void MainMenuShow()
{
    Console.Write("Логин: ");
    var login = Console.ReadLine();

    Console.Write("Пароль: ");
    var password = Console.ReadLine();

    Console.WriteLine(login);
    Console.WriteLine(password);
}