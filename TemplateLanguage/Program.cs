using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemplateLanguage
{
    class Program
    {
        static void Main(string[] args)
        {
            //Application.SetCompatibleTextRenderingDefault(false);
            Extensions.currentStringFormat = (StringFormat)StringFormat.GenericTypographic.Clone();
            Extensions.currentStringFormat.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            Application.EnableVisualStyles();
            Application.Run(new frmMain());
        }
    }
}
