#if !BRIDGE
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#else
using ExpressCraft;
using System.Collections.Generic;
#endif

namespace TemplateLanguage
{
    public static class Extensions
    {
        public static StringFormat currentStringFormat;
        
        public static bool CircleContains(this RectangleF rec, float x, float y)
        {            
            GraphicsPath myPath = new GraphicsPath();
            myPath.AddEllipse(rec);
            return myPath.IsVisible(x, y);
        }        
        public static string ToTitleCase(this string value)
        {
            // Creates a TextInfo based on the "en-US" culture.
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            
            return char.ToUpper(value[0]) + value.Substring(1);
        }
        public static SizeF MesaureStringPixel(this Graphics g, string text, Font font)
        {            
            return g.MeasureString(text.Replace("\t", "    "), font, int.MaxValue, currentStringFormat);
            //var m = TextRenderer.MeasureText(g, text, font, new Size(width, 19), TextFormatFlags.Left | TextFormatFlags.NoPadding);
            //return new SizeF(m.Width, m.Height);
           // return TextRenderer.MeasureText(g, text, font);
        }

        public static void DrawText(this Graphics g, string text, Font font, SolidBrush color, RectangleF bounds)
        {
            //g.DrawString(String.Format(" {0}", i + 1), Font, LineCountColor, 0.0F, cachedY, sf)
            g.DrawString(text.Replace("\t", "    "), font, color, bounds.X, bounds.Y, currentStringFormat);
            //TextRenderer.DrawText(g, text, font, new Rectangle((int)bounds.X, (int)bounds.Y, (int)bounds.Width, (int)bounds.Height), color.Color, TextFormatFlags.Left | TextFormatFlags.NoPadding); // , TextFormatFlags.NoPadding | TextFormatFlags.NoClipping
        }
    }
}
