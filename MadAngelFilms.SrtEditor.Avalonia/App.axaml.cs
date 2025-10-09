using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using MadAngelFilms.SrtEditor.Avalonia.ViewModels;
using MadAngelFilms.SrtEditor.Avalonia.Views;
using MadAngelFilms.SrtEditor.Controllers;
using MadAngelFilms.SrtEditor.Core.Services;

namespace MadAngelFilms.SrtEditor.Avalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var controller = new MainWindowController(new SubtitleService());
            var viewModel = new MainWindowViewModel(controller);
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewModel
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
