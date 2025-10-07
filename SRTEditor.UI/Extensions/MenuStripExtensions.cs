using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MadAngelFilms.SrtEditor.UI.Styling;

namespace MadAngelFilms.SrtEditor.UI.Extensions;

internal static class MenuStripExtensions
{
    private static readonly Color BoneWhite = Color.FromArgb(0xF2, 0xF2, 0xF0);
    private static readonly Color Charcoal = Color.FromArgb(0x1A, 0x1A, 0x1A);
    private static readonly Color MidnightBlue = Color.FromArgb(0x0B, 0x1D, 0x3A);

    public static void ApplyMadAngelTheme(this MenuStrip menuStrip)
    {
        ArgumentNullException.ThrowIfNull(menuStrip);

        var colorTable = new MadAngelMenuColorTable();
        var renderer = new ToolStripProfessionalRenderer(colorTable)
        {
            RoundedEdges = false
        };

        menuStrip.RenderMode = ToolStripRenderMode.Professional;
        menuStrip.Renderer = renderer;
        menuStrip.BackColor = Charcoal;
        menuStrip.ForeColor = BoneWhite;
        menuStrip.GripMargin = Padding.Empty;
        menuStrip.AutoSize = false;
        menuStrip.Padding = new Padding(18, 12, 18, 10);
        menuStrip.Height = menuStrip.Font.Height + menuStrip.Padding.Vertical + 8;
        menuStrip.ShowItemToolTips = true;

        foreach (var menuItem in menuStrip.Items.OfType<ToolStripMenuItem>())
        {
            StyleMenuItem(menuItem, isTopLevel: true, renderer);
        }
    }

    private static void StyleMenuItem(ToolStripMenuItem menuItem, bool isTopLevel, ToolStripProfessionalRenderer renderer)
    {
        menuItem.ForeColor = BoneWhite;
        menuItem.BackColor = isTopLevel ? Charcoal : MidnightBlue;
        menuItem.Margin = Padding.Empty;
        menuItem.Padding = isTopLevel ? new Padding(14, 6, 14, 6) : new Padding(12, 6, 12, 6);
        menuItem.ImageScaling = ToolStripItemImageScaling.None;
        menuItem.TextAlign = ContentAlignment.MiddleLeft;

        if (menuItem.DropDown is ToolStripDropDownMenu dropDownMenu)
        {
            dropDownMenu.BackColor = MidnightBlue;
            dropDownMenu.ForeColor = BoneWhite;
            dropDownMenu.ShowImageMargin = true;
            dropDownMenu.Renderer = renderer;
            dropDownMenu.Padding = new Padding(4, 6, 4, 6);

            foreach (ToolStripItem item in dropDownMenu.Items)
            {
                switch (item)
                {
                    case ToolStripMenuItem childMenuItem:
                        StyleMenuItem(childMenuItem, isTopLevel: false, renderer);
                        break;
                    case ToolStripSeparator separator:
                        separator.Margin = new Padding(12, 6, 12, 6);
                        break;
                }
            }
        }
    }
}
