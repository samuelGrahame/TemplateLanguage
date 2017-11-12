using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateLanguage
{
    public class Value : BaseTemplate
    {
        public Field Owner;
        public bool AllSelected = false;
        public Value(Field owner)
        {
            Owner = owner;
        }
    }
}
