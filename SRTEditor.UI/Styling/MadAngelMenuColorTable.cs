using System.Drawing;
using System.Windows.Forms;

namespace MadAngelFilms.SrtEditor.UI.Styling;

internal sealed class MadAngelMenuColorTable : ProfessionalColorTable
{
    private static readonly Color Charcoal = Color.FromArgb(0x1A, 0x1A, 0x1A);
    private static readonly Color MidnightBlue = Color.FromArgb(0x0B, 0x1D, 0x3A);
    private static readonly Color RustRed = Color.FromArgb(0x99, 0x2A, 0x1C);
    private static readonly Color BloodRed = Color.FromArgb(0x7C, 0x0A, 0x02);
    private static readonly Color AgedGold = Color.FromArgb(0xC2, 0x9F, 0x13);
    private static readonly Color CrimsonShadow = Color.FromArgb(0x4A, 0x00, 0x00);

    public override Color MenuStripGradientBegin => Charcoal;

    public override Color MenuStripGradientEnd => Charcoal;

    public override Color ToolStripDropDownBackground => MidnightBlue;

    public override Color ImageMarginGradientBegin => MidnightBlue;

    public override Color ImageMarginGradientMiddle => MidnightBlue;

    public override Color ImageMarginGradientEnd => MidnightBlue;

    public override Color MenuBorder => CrimsonShadow;

    public override Color MenuItemBorder => AgedGold;

    public override Color MenuItemSelected => RustRed;

    public override Color MenuItemSelectedGradientBegin => RustRed;

    public override Color MenuItemSelectedGradientEnd => RustRed;

    public override Color MenuItemPressedGradientBegin => BloodRed;

    public override Color MenuItemPressedGradientEnd => BloodRed;

    public override Color SeparatorDark => CrimsonShadow;

    public override Color SeparatorLight => AgedGold;

    public override Color ToolStripBorder => Charcoal;
}
