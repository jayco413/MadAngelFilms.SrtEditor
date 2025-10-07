using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using MadAngelFilms.SrtEditor.Core.Models;

namespace MadAngelFilms.SrtEditor.Core.Services;

public sealed class SubtitleService : ISubtitleService
{
    public async Task<SubtitleProject> LoadProjectAsync(string directoryPath, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(directoryPath);

        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Directory '{directoryPath}' was not found.");
        }

        string? subtitlePath = Directory.EnumerateFiles(directoryPath, "*.srt", SearchOption.TopDirectoryOnly)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault();

        string? videoPath = Directory.EnumerateFiles(directoryPath, "*.mkv", SearchOption.TopDirectoryOnly)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault();

        if (subtitlePath is null || videoPath is null)
        {
            throw new InvalidOperationException("The selected directory must contain at least one .srt file and one .mkv file.");
        }

        IReadOnlyList<SubtitleEntry> entries = await ParseSubtitleFileAsync(subtitlePath, cancellationToken).ConfigureAwait(false);

        return new SubtitleProject(subtitlePath, videoPath, entries);
    }

    private static async Task<IReadOnlyList<SubtitleEntry>> ParseSubtitleFileAsync(string subtitlePath, CancellationToken cancellationToken)
    {
        var entries = new List<SubtitleEntry>();
        using FileStream stream = new(subtitlePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new StreamReader(stream, Encoding.UTF8, true);

        string? line;
        while ((line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) != null)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (!int.TryParse(line, NumberStyles.Integer, CultureInfo.InvariantCulture, out int sequenceNumber))
            {
                continue;
            }

            string? timingLine = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
            if (timingLine is null)
            {
                break;
            }

            (TimeSpan start, TimeSpan end) = ParseTimingLine(timingLine);

            var textBuilder = new StringBuilder();
            while ((line = await reader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) != null && !string.IsNullOrWhiteSpace(line))
            {
                textBuilder.AppendLine(line.Trim());
            }

            string text = textBuilder.ToString().TrimEnd();
            entries.Add(new SubtitleEntry(sequenceNumber, start, end, text));
        }

        return entries;
    }

    private static (TimeSpan Start, TimeSpan End) ParseTimingLine(string timingLine)
    {
        string[] parts = timingLine.Split("-->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            throw new FormatException($"Invalid timing line: {timingLine}");
        }

        return (ParseTime(parts[0]), ParseTime(parts[1]));
    }

    private static TimeSpan ParseTime(string text)
    {
        return TimeSpan.ParseExact(text, @"hh\:mm\:ss\,fff", CultureInfo.InvariantCulture);
    }
}
