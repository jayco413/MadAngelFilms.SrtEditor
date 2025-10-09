using MadAngelFilms.SrtEditor.Core.Models;
using MadAngelFilms.SrtEditor.Core.Services;

namespace MadAngelFilms.SrtEditor.Controllers;

public sealed class MainWindowController
{
    private readonly ISubtitleService _subtitleService;

    public MainWindowController(ISubtitleService subtitleService)
    {
        _subtitleService = subtitleService;
    }

    public Task<SubtitleProject> LoadProjectAsync(string directoryPath, CancellationToken cancellationToken)
    {
        return _subtitleService.LoadProjectAsync(directoryPath, cancellationToken);
    }
}
