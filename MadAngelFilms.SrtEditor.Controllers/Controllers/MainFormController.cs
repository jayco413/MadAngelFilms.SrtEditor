using MadAngelFilms.SrtEditor.Core.Models;
using MadAngelFilms.SrtEditor.Core.Services;

namespace MadAngelFilms.SrtEditor.Controllers;

public sealed class MainFormController
{
    private readonly ISubtitleService _subtitleService;

    public MainFormController(ISubtitleService subtitleService)
    {
        _subtitleService = subtitleService;
    }

    public Task<SubtitleProject> LoadProjectAsync(string directoryPath, CancellationToken cancellationToken)
    {
        return _subtitleService.LoadProjectAsync(directoryPath, cancellationToken);
    }
}
