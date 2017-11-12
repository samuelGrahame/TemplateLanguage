using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateLanguage
{
    public class Function : BaseTemplate
    {
        public Type ReturnType;
        public Field[] ReceiveTypes = null;
        public FunctionBody Body;

        public float Width;
        public float Height;

        public Function()
        {
            Body = new FunctionBody(this);
        }

        public void CalculateBounds()
        {
            Height = 19 + ((Body.Rows.Count == 0 ? 1 : Body.Rows.Count) * 21) + 3;
        }
    }
}
