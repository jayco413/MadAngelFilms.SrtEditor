using MadAngelFilms.SrtEditor.Core.Models;

namespace MadAngelFilms.SrtEditor.Core.Services;

public interface ISubtitleService
{
    Task<SubtitleProject> LoadProjectAsync(string directoryPath, CancellationToken cancellationToken);
}
