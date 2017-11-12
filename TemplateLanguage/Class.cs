using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemplateLanguage
{
    public class Class : BaseTemplate
    {        
        private BaseTemplate _focusedItem;

        public BaseTemplate FocusedItem
        {
            get { return _focusedItem; }
            set {
                if (_focusedItem != value)
                {
                    if (_focusedItem != null)
                    {
                        _focusedItem.IsFocused = false;
                    }
                    _focusedItem = value;
                    if (_focusedItem != null)
                    {
                        _focusedItem.IsFocused = true;                        
                    }                   
                }
            }
        }
        
        public float X;
        public float Y;

        public float Height;
        public float Width;        
        
        public List<Field> Fields = new List<Field>();
        public List<Function> Functions = new List<Function>();

        public override void OnPaint(Graphics g, TemplateControl control)
        {
            // #TODO# - Cache Results / When Change Record..
            CalculateBounds();
            // render class header then the content
            // we are going to use the forecolor setting from the color
            using (SolidBrush primaryColor = new SolidBrush(control.PrimaryColor))
            using (SolidBrush primaryHoverColor = new SolidBrush(control.PrimaryHoverColor))
            using (Pen primaryPen = new Pen(primaryColor, 2))
            using (Pen primaryHoverPen = new Pen(primaryHoverColor, 2))            
            using (SolidBrush textColor = new SolidBrush(control.ForeColor))
            using (SolidBrush backColor = new SolidBrush(control.ItemBackColor))
            using (Pen bodyRowFocused = new Pen(backColor, 2))
            using (SolidBrush itemTypeColor = new SolidBrush(control.ItemTypeColor))
            using (SolidBrush itemHoverColor = new SolidBrush(control.ItemHoverColor))
            using (SolidBrush secondaryColor = new SolidBrush(control.SecondaryColor))
            using (SolidBrush stringColor = new SolidBrush(control.StringColor))
            using (SolidBrush numberColor = new SolidBrush(control.NumberColor))
            using (SolidBrush itemFocusedColor = new SolidBrush(control.ItemFocusedBackColor))
            using (SolidBrush itemSelectedColor = new SolidBrush(control.TextEditorSelectionColor))
            using (Pen notFocusedPen = new Pen(backColor, 2))
            {
                g.FillRectangle(secondaryColor, X, Y, Width, Height);
                g.DrawRectangle(IsHovered ? primaryHoverPen : IsFocused ? primaryPen : notFocusedPen, X, Y, Width, Height);
                float y = Y;
                g.FillRectangle(IsHovered ? primaryHoverColor : IsFocused ? primaryColor : backColor, X, Y, Width, 19);
                g.DrawString(Name, control.Font, textColor, X + 3, Y + 3);

                y += 19;
                if (Fields != null)
                    foreach (var item in Fields)
                    {
                        y += 2;
                        g.FillRectangle(item.IsFocused || (item.InitalValue != null && item.InitalValue.IsFocused) ? itemFocusedColor : item.IsHovered ? itemHoverColor : backColor, X + 3, y, Width - 6, 19);
                        var size = g.MeasureString(item.Type.Name, control.Font);
                        g.DrawString(item.Type.Name, control.Font, itemTypeColor, new RectangleF(X + 6, y + 3, size.Width, 19 - 3));
                        g.DrawString(item.Name, control.Font, textColor, new RectangleF(X + 6 + size.Width + 3, y + 3, Width - 12 - size.Width - 3, 19 - 3));

                        if (item.InitalValue != null && !string.IsNullOrWhiteSpace(item.InitalValue.Name))
                        {
                            bool printValue = false;
                            SolidBrush solidbrush = null;
                            string drawValue = "";
                            if (item.Type == typeof(string))
                            {
                                solidbrush = stringColor;
                                printValue = true;
                                drawValue = "\"" + item.InitalValue.Name + "\"";
                            }
                            else if (item.Type == typeof(Int32) || item.Type == typeof(decimal))
                            {
                                solidbrush = numberColor;
                                printValue = true;
                                drawValue = item.InitalValue.Name;
                            }
                            if (printValue)
                            {
                                var size2 = g.MeasureString(item.Name, control.Font);
                                g.DrawString(drawValue, control.Font, solidbrush, new RectangleF(X + 6 + size.Width + 3 + size2.Width + 3, y + 3, Width - 12 - size.Width - 3 - size2.Width - 3, 19 - 3));
                            }
                        }

                        y += 19;
                    }

                if (Functions != null)
                    foreach (var item in Functions)
                    {
                        y += 2;
                        g.FillRectangle(item.IsFocused || (item.Body != null && item.Body.IsFocused) ? itemFocusedColor : item.IsHovered ? itemHoverColor : backColor, X + 3, y, Width - 6, 19);
                        var size = g.MeasureString(item.ReturnType.Name, control.Font);
                        g.DrawString(item.ReturnType.Name, control.Font, itemTypeColor, new RectangleF(X + 6, y + 3, size.Width, 19 - 3));

                        g.DrawString(item.Name, control.Font, textColor, new RectangleF(X + 6 + size.Width + 3, y + 3, Width - 12 - size.Width - 3, 19 - 3));
                        y += 19;
                    }

                Function func = null;

                if (_focusedItem is Function)
                {
                    func = (Function)_focusedItem;
                }
                else if (_focusedItem is FunctionBody)
                {
                    func = ((FunctionBody)_focusedItem).Owner;
                }

                if (func != null)
                {
                    func.CalculateBounds();

                    g.FillRectangle(secondaryColor, X + Width, Y, func.Width, func.Height);
                    g.DrawRectangle(func.Body.IsHovered ? primaryHoverPen : func.Body.IsFocused ? primaryPen : notFocusedPen, X + Width, Y, func.Width, func.Height);
                    g.FillRectangle(func.Body.IsHovered ? primaryHoverColor : func.Body.IsFocused ? primaryColor : backColor, X + Width, Y, func.Width, 19);
                    g.DrawString(func.Name, control.Font, textColor, X + Width + 3, Y + 3);

                    y = Y;
                    y += 19;
                    int index = 0;
                    int intWidth = (int)func.Width;

                    int columnIndex = func.Body.ColumnIndex;
                    
                    if (func.Body.Rows.Count > 0 && columnIndex > func.Body.Rows[func.Body.RowIndex].Name.Length)
                    {
                        columnIndex = func.Body.Rows[func.Body.RowIndex].Name.Length;
                    }

                    foreach (var line in func.Body.Rows)
                    {
                        y += 2;

                        if (func.Body.SelectionRowIndex != -1 && line.Name.Length > 0)
                        {
                            if (index > func.Body.SelectionRowIndex && index < func.Body.RowIndex)
                            {
                                var sizeOfPrint = g.MesaureStringTypographic(line.Name, control.Font);
                                g.FillRectangle(itemSelectedColor, X + Width + 2, y - 2, sizeOfPrint.Width, 22);
                            }
                            else if (index > func.Body.RowIndex && index < func.Body.SelectionRowIndex)
                            {
                                var sizeOfPrint = g.MesaureStringTypographic(line.Name, control.Font);
                                g.FillRectangle(itemSelectedColor, X + Width + 2, y - 2, sizeOfPrint.Width, 22);
                            }

                            if (index == func.Body.RowIndex && index == func.Body.SelectionRowIndex)
                            {
                                if (func.Body.SelectionColumnIndex < columnIndex)
                                {
                                    var sizeOfNonPrint = g.MesaureStringTypographic(func.Body.SelectionColumnIndex == 0 ? "" : line.Name.Substring(0, func.Body.SelectionColumnIndex), control.Font);
                                    var sizeOfPrint = g.MesaureStringTypographic(line.Name.Substring(func.Body.SelectionColumnIndex, columnIndex - func.Body.SelectionColumnIndex), control.Font);

                                    g.FillRectangle(itemSelectedColor, X + Width + 2 + sizeOfNonPrint.Width, y - 2, sizeOfPrint.Width, 22);
                                }
                                else
                                {
                                    var sizeOfNonPrint = g.MesaureStringTypographic(line.Name.Substring(0, columnIndex), control.Font);
                                    var sizeOfPrint = g.MesaureStringTypographic(line.Name.Substring(columnIndex, func.Body.SelectionColumnIndex - columnIndex), control.Font);

                                    g.FillRectangle(itemSelectedColor, X + Width + 2 + sizeOfNonPrint.Width, y - 2, sizeOfPrint.Width, 22);
                                }
                            }
                            else
                            {
                                if (index == func.Body.RowIndex)
                                {
                                    if (func.Body.SelectionRowIndex < func.Body.RowIndex)
                                    {
                                        if (columnIndex != 0)
                                        {
                                            var sizeOfPrint = g.MesaureStringTypographic(line.Name.Substring(0, columnIndex), control.Font);
                                            g.FillRectangle(itemSelectedColor, X + Width + 2, y - 2, sizeOfPrint.Width, 22);
                                        }
                                    }
                                    else
                                    {
                                        if (columnIndex == 0)
                                        {
                                            var sizeOfPrint = g.MeasureString(line.Name, control.Font);
                                            g.FillRectangle(itemSelectedColor, X + Width + 2, y - 2, sizeOfPrint.Width, 22);
                                        }
                                        else
                                        {
                                            var sizeOfNonPrint = g.MesaureStringTypographic(line.Name.Substring(0, columnIndex), control.Font);
                                            var sizeOfPrint = g.MesaureStringTypographic(line.Name.Substring(columnIndex), control.Font);
                                            g.FillRectangle(itemSelectedColor, X + Width + 2 + sizeOfNonPrint.Width, y - 2, sizeOfPrint.Width, 22);
                                        }
                                    }
                                }
                                else if (index == func.Body.SelectionRowIndex)
                                {
                                    if (func.Body.SelectionRowIndex < func.Body.RowIndex)
                                    {
                                        if (func.Body.SelectionColumnIndex == 0)
                                        {
                                            var sizeOfPrint = g.MesaureStringTypographic(line.Name, control.Font);
                                            g.FillRectangle(itemSelectedColor, X + Width + 2, y - 2, sizeOfPrint.Width, 22);
                                        }
                                        else
                                        {
                                            var sizeOfNonPrint = g.MesaureStringTypographic(func.Body.SelectionColumnIndex == 0 ? "" : line.Name.Substring(0, func.Body.SelectionColumnIndex), control.Font);
                                            var sizeOfPrint = g.MesaureStringTypographic(line.Name.Substring(func.Body.SelectionColumnIndex, line.Name.Length - func.Body.SelectionColumnIndex), control.Font);

                                            g.FillRectangle(itemSelectedColor, X + Width + 2 + sizeOfNonPrint.Width, y - 2, sizeOfPrint.Width, 22);
                                        }
                                    }
                                    else
                                    {
                                        if (func.Body.SelectionColumnIndex != 0)
                                        {
                                            var sizeOfPrint = g.MesaureStringTypographic(line.Name.Substring(0, func.Body.SelectionColumnIndex), control.Font);
                                            g.FillRectangle(itemSelectedColor, X + Width + 2, y - 2, sizeOfPrint.Width, 22);
                                        }
                                    }
                                }
                            }
                        }



                        if (index == func.Body.RowIndex)
                        {
                            if (func.Body.SelectionRowIndex == -1)
                                g.DrawRectangle(bodyRowFocused, X + Width + 2, y - 1, func.Width - 4, 19);

                            using (Pen cursorPen = new Pen(control.ForeColor))
                            {
                                int colindex = func.Body.ColumnIndex;
                                if (colindex > line.Name.Length)
                                {
                                    colindex = line.Name.Length;
                                }
                                var size3 = g.MesaureStringTypographic(colindex == 0 ? "" : line.Name.Substring(0, colindex), control.Font);
                                
                                g.DrawLine(cursorPen, (float)(X + Width + 2 + size3.Width), (float)(y - 1), (float)(X + Width + 2 + size3.Width), (float)(y + 19 - 1));
                            }
                        }
                        
                        g.DrawTextTypographic(line.Name, control.Font, textColor, new RectangleF(X + Width + 2, y + 3, func.Width - 4, 19 - 3));

                        y += 19;
                        index++;
                    }
                }
            }
        }

        public void CalculateBounds()
        {
            Height = 19 + ((Fields.Count + Functions.Count) * 21) + 3;
        }
    }
}
