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
        
        /// <summary>
        /// Uses GraphicsPath
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// GenericTypographic is used, Tab is replaced with 4 spaces
        /// </summary>
        /// <param name="g"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static SizeF MesaureStringTypographic(this Graphics g, string text, Font font)
        {            
            return g.MeasureString(text.Replace("\t", "    "), font, int.MaxValue, currentStringFormat);           
        }

        /// <summary>
        /// GenericTypographic is used, Tab is replaced with 4 spaces
        /// </summary>
        /// <param name="g"></param>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="color"></param>
        /// <param name="bounds"></param>
        public static void DrawTextTypographic(this Graphics g, string text, Font font, SolidBrush color, RectangleF bounds)
        {            
            g.DrawString(text.Replace("\t", "    "), font, color, bounds.X, bounds.Y, currentStringFormat);            
        }
    }
}
