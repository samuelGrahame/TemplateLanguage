using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateLanguage
{
    public class FunctionBody : BaseTemplate
    {
        public Function Owner;
        public List<FunctionRow> Rows = new List<FunctionRow>();
        public int RowIndex = 0;
        public int ColumnIndex = 0;

        public int SelectionRowIndex = -1;
        public int SelectionColumnIndex = -1;
        
        public FunctionBody(Function owner)
        {
            Owner = owner;
        }
    }
}
