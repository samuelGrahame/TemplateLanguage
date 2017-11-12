using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateLanguage
{
    public class BaseTemplate
    {
        public string Name;
        public bool IsHovered;
        public bool IsFocused;

        public virtual void OnPaint(Graphics g, TemplateControl control)
        {

        }
    }
}
