using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.IO;
using System.Reflection;
using Svg;

namespace MadAngelFilms.SrtEditor.UI.Resources;

internal static class IconProvider
{
    private const string ResourcePrefix = "MadAngelFilms.SrtEditor.UI.Resources.Icons.";
    private static readonly ConcurrentDictionary<string, Image> IconCache = new();

    public static Image GetIcon(string iconName, int size)
    {
        if (string.IsNullOrWhiteSpace(iconName))
        {
            throw new ArgumentException("Icon name must be provided.", nameof(iconName));
        }

        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "Icon size must be greater than zero.");
        }

        string cacheKey = $"{iconName}-{size}";
        return IconCache.GetOrAdd(cacheKey, _ => LoadIcon(iconName, size));
    }

    private static Image LoadIcon(string iconName, int size)
    {
        string resourceName = ResourcePrefix + iconName + ".svg";
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream? resourceStream = assembly.GetManifestResourceStream(resourceName);
        if (resourceStream is null)
        {
            throw new InvalidOperationException($"Unable to locate icon resource '{resourceName}'.");
        }

        SvgDocument document = SvgDocument.Open<SvgDocument>(resourceStream);
        document.ShapeRendering = SvgShapeRendering.Auto;
        Bitmap bitmap = document.Draw(size, size);
        bitmap.MakeTransparent(Color.Transparent);
        return bitmap;
    }
}
