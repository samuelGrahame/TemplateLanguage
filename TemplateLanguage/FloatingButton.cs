using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateLanguage
{
    public class FloatingButton : BaseTemplate
    {
        public Color BackColor = Color.Transparent;

        public Color GetBackColor(Color primaryColor)
        {
            return BackColor == Color.Transparent ? primaryColor : BackColor;
        }

        public Action<Class, TemplateControl> OnClick = null;
        public Bitmap Icon;
    }
}
