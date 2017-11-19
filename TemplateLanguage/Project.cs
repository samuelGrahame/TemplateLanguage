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
    public class Project
    {
        public float X;
        public float Y;
        public List<Class> Classes = new List<Class>();
    }
}
