using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TemplateLanguage.Properties;

namespace TemplateLanguage
{
    public class TemplateControl : Control
    {
        private bool _isMouseDown;
        private bool _didClickOnInnerItems = false;
        private float _mouseX;
        private float _mouseY;
        public Project Current;
        public FloatingButton FloatingMenu = new FloatingButton();

        public List<FloatingButton> Vertical = new List<FloatingButton>();
        public List<FloatingButton> Horizontal = new List<FloatingButton>();

        public Bitmap add_Icon;
        public Bitmap minus_Icon;
        public Bitmap cSharp_Icon;
        public Bitmap number_Icon;
        public Bitmap currency_Icon;
        public Bitmap string_Icon;
        public Bitmap date_Icon;
        public Bitmap delete_Icon;
        public Bitmap sendtoBack_Icon;
        public Bitmap bringtoFront_Icon;
        public Bitmap function_Icon;       

        protected override bool IsInputKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
                return true;

            if ((keyData == Keys.Tab || keyData == Keys.ShiftKey) && FocusedClass != null && FocusedClass.FocusedItem != null && FocusedClass.FocusedItem is FunctionBody)
                return true;

            return base.IsInputKey(keyData);
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            if (FocusedClass != null)
            {
                BaseTemplate template = FocusedClass;
                if (FocusedClass.FocusedItem != null)
                {
                    template = FocusedClass.FocusedItem;

                    if (template is FunctionBody)
                    {
                        ProcessKeyDownOnCodeEditor(e, (FunctionBody)template);
                        return;
                    }
                }

                if (e.KeyCode == Keys.Delete)
                {
                    if (e.Shift)
                    {
                        // #TODO# - Remove Selected Row
                    }
                    else
                    {
                        template.Name = "";
                    }

                    Refresh();
                }
                else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
                {
                    if (template is Field)
                    {
                        var index = FocusedClass.Fields.IndexOf((Field)template);
                        if (index >= 0)
                        {
                            if (index == FocusedClass.Fields.Count - 1 && e.KeyCode != Keys.Up)
                            {
                                // last one move to functions
                            }
                            else if (index == 0 && e.KeyCode == Keys.Up)
                            {

                            }
                            else
                            {
                                FocusedClass.FocusedItem = FocusedClass.Fields[e.KeyCode == Keys.Up ? index - 1 : index + 1];
                                Refresh();
                            }
                        }
                    }
                }


            }
            base.OnPreviewKeyDown(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            
            base.OnKeyDown(e);
        }

        private void ProcessKeyDownOnCodeEditor(PreviewKeyDownEventArgs e, FunctionBody body)
        {
            if (body == null)
                return;

            if(e.KeyCode == Keys.Enter)
            {
                if (body.Rows.Count == 0)
                {
                    body.RowIndex = 0;
                    body.ColumnIndex = 0;
                    body.Rows.Add(new FunctionRow());                    
                }
                else
                {
                    if (body.ColumnIndex > body.Rows[body.RowIndex].Name.Length)
                    {
                        body.ColumnIndex = body.Rows[body.RowIndex].Name.Length;
                    }

                    if(body.ColumnIndex == body.Rows[body.RowIndex].Name.Length)
                    {
                        body.RowIndex++;
                        body.Rows.Insert(body.RowIndex, new FunctionRow());
                        body.ColumnIndex = 0;
                    }
                    else
                    {
                        var row = body.Rows[body.RowIndex];

                        var name = row.Name;

                        row.Name = name.Substring(0, body.ColumnIndex);

                        body.RowIndex++;
                        body.Rows.Insert(body.RowIndex, new FunctionRow() { Name = name.Substring(body.ColumnIndex) });
                        body.ColumnIndex = 0;
                    }

                    
                }

                Refresh();
            }else if (e.KeyCode == Keys.Up)
            {
                int prevCol = body.ColumnIndex;
                int prevRow = body.RowIndex;

                body.RowIndex--;
                if (body.RowIndex < 0)
                    body.RowIndex = 0;

                if (e.Shift)
                {
                    if (body.SelectionRowIndex == -1)
                    {
                        body.SelectionRowIndex = prevRow;
                        body.SelectionColumnIndex = prevCol;
                    }
                }
                else
                {
                    body.SelectionRowIndex = -1;
                    body.SelectionColumnIndex = -1;
                }

                Refresh();
            }
            else if (e.KeyCode == Keys.Down)
            {
                int prevCol = body.ColumnIndex;
                int prevRow = body.RowIndex;

                body.RowIndex++;
                if (body.RowIndex >= body.Rows.Count)
                    body.RowIndex = body.Rows.Count - 1;


                if (e.Shift)
                {
                    if (body.SelectionRowIndex == -1)
                    {
                        body.SelectionRowIndex = prevRow;
                        body.SelectionColumnIndex = prevCol;
                    }
                }
                else
                {
                    body.SelectionRowIndex = -1;
                    body.SelectionColumnIndex = -1;
                }

                Refresh();
            }
            else if (e.KeyCode == Keys.Left)
            {
                      
                if (body.Rows.Count == 0)
                    return;                               

                if (body.ColumnIndex > body.Rows[body.RowIndex].Name.Length)
                {
                    body.ColumnIndex = body.Rows[body.RowIndex].Name.Length;
                }

                int prevCol = body.ColumnIndex;
                int prevRow = body.RowIndex;

                body.ColumnIndex--;
                if(body.ColumnIndex < 0)
                {
                    body.RowIndex--;
                    if(body.RowIndex < 0)
                    {
                        body.RowIndex = 0;
                        body.ColumnIndex = 0;
                    }
                    else
                    {
                        body.ColumnIndex = body.Rows[body.RowIndex].Name.Length;
                    }                    
                }                

                if(e.Shift)
                {
                    if(body.SelectionRowIndex == -1)
                    {
                        body.SelectionRowIndex = prevRow;
                        body.SelectionColumnIndex = prevCol;
                    }
                }
                else
                {
                    body.SelectionRowIndex = -1;
                    body.SelectionColumnIndex = -1;
                }

                Refresh();
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (body.Rows.Count == 0)
                    return;

                if (body.ColumnIndex > body.Rows[body.RowIndex].Name.Length)
                {
                    body.ColumnIndex = body.Rows[body.RowIndex].Name.Length;
                }

                int prevCol = body.ColumnIndex;
                int prevRow = body.RowIndex;

                body.ColumnIndex++;
                if (body.ColumnIndex > body.Rows[body.RowIndex].Name.Length)
                {
                    body.RowIndex++;
                    if (body.RowIndex >= body.Rows.Count)
                    {
                        body.RowIndex = body.Rows.Count - 1;
                        body.ColumnIndex = body.Rows[body.RowIndex].Name.Length;
                    }
                    else
                    {
                        body.ColumnIndex = 0;
                    }                    
                }

                if (e.Shift)
                {
                    if (body.SelectionRowIndex == -1)
                    {
                        body.SelectionRowIndex = prevRow;
                        body.SelectionColumnIndex = prevCol;
                    }
                }
                else
                {
                    body.SelectionRowIndex = -1;
                    body.SelectionColumnIndex = -1;
                }

                Refresh();
            }
            else if (e.KeyCode == Keys.End)
            {
                if (body.Rows.Count > 0)
                {
                    body.ColumnIndex = body.Rows[body.RowIndex].Name.Length;
                    Refresh();
                }
            }
            else if (e.KeyCode == Keys.Home)
            {                
                body.ColumnIndex = 0;
                Refresh();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (body.Rows.Count == 0)
                    return;
                var row = body.Rows[body.RowIndex];
                if (row.Name.Length == 0)
                {
                   // body.RowIndex++;
                    body.Rows.Remove(row);
                    body.ColumnIndex = 0;
                }
                else
                {
                    int index = body.ColumnIndex;
                    if (body.ColumnIndex > row.Name.Length)
                    {
                        index = row.Name.Length;
                    }
                    if(index == row.Name.Length)
                    {
                        if(body.RowIndex == body.Rows.Count - 1)
                        {
                            // last row??
                        }
                        else
                        {
                            var row2 = body.Rows[body.RowIndex + 1];
                            body.Rows.Remove(row2);
                            row.Name += row2.Name;
                        }
                    }
                    else
                    {
                        row.Name = row.Name.Remove(index, 1);
                    }                    
                }

                Refresh();
            }

        }

        private void ProcessKeyPressOnCodeEditor(KeyPressEventArgs e, FunctionBody body)
        {
            if (body == null)
                return;
            if (e.KeyChar == '\r')
                return;

            if (e.KeyChar == '\b')
            {
                if (body.Rows.Count == 0)
                    return;
                var row = body.Rows[body.RowIndex];
                if (row.Name.Length == 0)
                {
                    body.RowIndex--;
                    body.Rows.Remove(row);
                    if(body.Rows.Count > 0)
                    {
                        body.ColumnIndex = body.Rows.Last().Name.Length;
                    }
                    else
                    {
                        body.ColumnIndex = 0;
                    }
                }
                else
                {
                    int index = body.ColumnIndex;
                    if (body.ColumnIndex > row.Name.Length)
                    {
                        index = row.Name.Length;
                    }
                    if (index - 1 < 0)
                    {
                        if(body.RowIndex > 0)
                        {
                            body.RowIndex--;
                            body.Rows.Remove(row);
                            if (body.Rows.Count > 0)
                            {
                                body.ColumnIndex = body.Rows.Last().Name.Length;
                            }
                            else
                            {
                                body.ColumnIndex = 0;
                            }
                            if (body.RowIndex <= -1)
                            {
                                body.RowIndex = 0;
                            }
                            else
                            {
                                body.Rows[body.RowIndex].Name += row.Name;
                            }
                        }
                        
                    }
                    else
                    {
                        row.Name = row.Name.Remove(index - 1, 1);
                        body.ColumnIndex--;
                    }                 
                }
                
                Refresh();
            }
            else
            {
                if (body.Rows.Count == 0)
                {
                    body.RowIndex = 0;
                    body.ColumnIndex = 0;
                    body.Rows.Add(new FunctionRow());
                }
                var row = body.Rows[body.RowIndex];
                int index = body.ColumnIndex;
                if (body.ColumnIndex > row.Name.Length)
                {
                    index = row.Name.Length;
                }

                row.Name = row.Name.Insert(index, e.KeyChar.ToString());

                var size = this.CreateGraphics().MeasureString(row.Name, this.Font);

                if(size.Width + 12 > body.Owner.Width)
                {
                    body.Owner.Width = size.Width + 12;
                }

                body.ColumnIndex++;

                Refresh();
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if(FocusedClass != null)
            {
                BaseTemplate template = FocusedClass;
                if(FocusedClass.FocusedItem != null)
                {
                    template = FocusedClass.FocusedItem;

                    if(template is FunctionBody)
                    {
                        // Process the key press for the code editor.
                        ProcessKeyPressOnCodeEditor(e, (FunctionBody)template);
                        return;
                    }
                }

                if(e.KeyChar == '\b')
                {
                    if(template.Name.Length > 0)
                        template.Name = template.Name.Remove(template.Name.Length - 1, 1);
                }
                else
                {
                    if(e.KeyChar == '=' && template is Field)
                    {
                        var field = (Field)template;
                        if(field.InitalValue == null)
                        {
                            field.InitalValue = new Value(field);
                        }
                        FocusedClass.FocusedItem = field.InitalValue;
                        return;
                    }

                    if(template is Value)
                    {
                        var vl = (Value)template;
                        if(vl.AllSelected)
                        {
                            vl.AllSelected = false;
                            vl.Name = "";
                        }
                        template.Name += e.KeyChar;
                    }
                    else
                    {
                        if (char.IsLetterOrDigit(e.KeyChar) || e.KeyChar == ' ' || e.KeyChar == '_' || e.KeyChar == '-')
                            template.Name += e.KeyChar == ' ' ? '_' : e.KeyChar == '-' ? '_' : e.KeyChar;
                        string name = (template.Name + "").Trim();
                        if (template is Class)
                        {
                            name = name.ToTitleCase();
                        }
                        template.Name = name;
                    }                    
                }
                
                Refresh();
            }


            base.OnKeyPress(e);
        }

        public Bitmap ChangeColor(Bitmap scrBitmap, Color newColor)
        {
            Bitmap newBitmap = new Bitmap(scrBitmap.Width, scrBitmap.Height);

            for (int x = 0; x < scrBitmap.Width; x++)
            {
                for (int y = 0; y < scrBitmap.Height; y++)
                {                    
                    //get the pixel from the scrBitmap image
                    var actualColor = scrBitmap.GetPixel(x, y);
                    // > 150 because.. Images edges can be of low pixel colr. if we set all pixel color to new then there will be no smoothness left.
                    newBitmap.SetPixel(x, y, Color.FromArgb(actualColor.A, newColor.R, newColor.G, newColor.B));
                }
            }

            return newBitmap;
        }

        private Class _focusedClass;

        public Class FocusedClass
        {
            get { return _focusedClass; }
            set
            {
                if (_focusedClass != value)
                {
                    if(_focusedClass != null)
                    {
                        _focusedClass.IsFocused = false;
                    }
                    _focusedClass = value;
                    if (_focusedClass != null)
                    {
                        _focusedClass.IsFocused = true;
                    }
                    Refresh();
                }
            }
        }

        private bool _isMenuVisible;

        public bool IsMenuVisible
        {
            get { return _isMenuVisible; }
            set {
                if(_isMenuVisible != value)
                {
                    _isMenuVisible = value;
                    Refresh();
                }
            }
        }

        private BaseTemplate _lastHovered = null;
        
        private Color _primaryColor;
        public Color PrimaryColor
        {
            get { return _primaryColor; }
            set {
                _primaryColor = value;
                // Refresh the visual display because we have changed the a color.
                Refresh();
            }
        }

        private Color _primaryHoverColor;
        public Color PrimaryHoverColor
        {
            get { return _primaryHoverColor; }
            set
            {
                _primaryHoverColor = value;
                // Refresh the visual display because we have changed the a color.
                Refresh();
            }
        }        

        private Color _secondaryColor;
        public Color SecondaryColor
        {
            get { return _secondaryColor; }
            set
            {
                _secondaryColor = value;
                // Refresh the visual display because we have changed the a color.
                Refresh();
            }
        }

        private Color _textEditorSelectionColor;
        public Color TextEditorSelectionColor
        {
            get { return _textEditorSelectionColor; }
            set
            {
                _textEditorSelectionColor = value;
                // Refresh the visual display because we have changed the a color.
                Refresh();
            }
        }

        private Color _itemBackColor;
        public Color ItemBackColor
        {
            get { return _itemBackColor; }
            set
            {
                _itemBackColor = value;
                // Refresh the visual display because we have changed the a color.
                Refresh();
            }
        }

        private Color _itemFocusedBackColor;
        public Color ItemFocusedBackColor
        {
            get { return _itemFocusedBackColor; }
            set
            {
                _itemFocusedBackColor = value;
                // Refresh the visual display because we have changed the a color.
                Refresh();
            }
        }

        private Color _itemTypeColor;
        public Color ItemTypeColor
        {
            get { return _itemTypeColor; }
            set
            {
                _itemTypeColor = value;
                // Refresh the visual display because we have changed the a color.
                Refresh();
            }
        }

        private Color itemHoverColor;
        public Color ItemHoverColor
        {
            get { return itemHoverColor; }
            set
            {
                itemHoverColor = value;
                // Refresh the visual display because we have changed the a color.
                Refresh();
            }
        }
        
        private Color _stringColor;
        public Color StringColor
        {
            get { return _stringColor; }
            set
            {                
                _stringColor = value;
                // Refresh the visual display because we have changed the a color.
                Refresh();
            }
        }

        private Color _numberColor;
        public Color NumberColor
        {
            get { return _numberColor; }
            set
            {
                _numberColor = value;
                // Refresh the visual display because we have changed the a color.
                Refresh();
            }
        }

        public bool UseBlack(Color c)
        {
            return ((int)Math.Sqrt(
            c.R * c.R * .299 +
            c.G * c.G * .587 +
            c.B * c.B * .114)) > 130;
        }

        public TemplateControl()
        {
            // Set up the styles so that we can use GDI Draw
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);

            add_Icon = ChangeColor(Resources.add_32x32, Color.White);
            minus_Icon = ChangeColor(Resources.cancel_32x32, Color.White);
            cSharp_Icon = ChangeColor(Resources.csharp_32x32, Color.White);
            number_Icon = ChangeColor(Resources.listnumbers_32x32, Color.White);
            string_Icon = ChangeColor(Resources.spellcheckasyoutype_32x32, Color.White);
            date_Icon = ChangeColor(Resources.switchtimescalesto_32x32, Color.White);
            currency_Icon = ChangeColor(Resources.currency_32x32, Color.White);
            delete_Icon = ChangeColor(Resources.close_32x32, Color.White);
            sendtoBack_Icon = ChangeColor(Resources.sendtoback_32x32, Color.White);
            bringtoFront_Icon = ChangeColor(Resources.bringtofront_32x32, Color.White);
            function_Icon = Resources.scripts_32x32; // ChangeColor(Resources.scripts_32x32, Color.White);
        }

        public virtual void OnFloatButtonClicked(FloatingButton button)
        {
            if (button != null && button.OnClick != null)
                button.OnClick(FocusedClass, this);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            float mouseX = e.X;
            float mouseY = e.Y;

            if (Current == null)
                return;

            mouseX -= Current.X;
            mouseY -= Current.Y;


            var cls = GetClassFromLocation(mouseX, mouseY);
            
            if (cls != null)
            {
                var clickedOnItem = GetClassItem(cls, mouseY, mouseX);

                if (clickedOnItem != null)
                {                    
                    if(clickedOnItem is Field)
                    {
                        var field = (Field)clickedOnItem;
                        if(field.InitalValue == null)
                        {
                            field.InitalValue = new Value(field);
                        }
                        else
                        {
                            field.InitalValue.AllSelected = true;
                        }

                        FocusedClass.FocusedItem = field.InitalValue;
                    }
                    else if (clickedOnItem is Function)
                    {
                        var func = (Function)clickedOnItem;

                        FocusedClass.FocusedItem = func.Body;
                    }
                    else if (clickedOnItem is FunctionBody)
                    {
                        var body = (FunctionBody)clickedOnItem;
                        var g = this.CreateGraphics();

                        float max = 0;

                        for (int i = 0; i < body.Rows.Count; i++)
                        {
                            var size = g.MeasureString(body.Rows[i].Name, this.Font);
                            if (size.Width > max)
                            {
                                max = size.Width;
                               
                            }
                        }

                        if(max != 0)
                        {
                            body.Owner.Width = max + 12;
                        }
                    }
                }
                else
                {
                    FocusedClass.FocusedItem = null;
                }
                Refresh();
            }

            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            float mouseX = e.X;
            float mouseY = e.Y;
            Class foundClass = null;

            if (IsMenuVisible)
            {                
                var item = GetButtonFromLocation(mouseX, mouseY);
                if (item != null)
                {
                    if(e.Button != MouseButtons.Right)
                    {
                        IsMenuVisible = false;
                    }                    
                    
                    OnFloatButtonClicked(item);
                    if (e.Button == MouseButtons.Right)
                    {
                        Refresh();
                    }
                    return;
                }
                if(GetFloatButtonLocation().Contains(mouseX, mouseY))
                {
                    IsMenuVisible = false;
                    return;
                }
                if (e.Button == MouseButtons.Right)
                {
                    foundClass = GetClassFromLocation(mouseX, mouseY);
                    if(foundClass == null)
                    {
                        IsMenuVisible = false;
                    }
                }
                else
                {
                    IsMenuVisible = false;
                }

                
            }
            else if (GetFloatButtonLocation().Contains(mouseX, mouseY))
            {
                IsMenuVisible = true;
                return;
            }

            if (Current == null)
                return;

            mouseX -= Current.X;
            mouseY -= Current.Y;

            FocusedClass = foundClass ?? GetClassFromLocation(mouseX, mouseY);
            _didClickOnInnerItems = false;

            if (FocusedClass != null)
            {
                var clickedOnItem = GetClassItem(FocusedClass, mouseY, mouseX);
                
                if (clickedOnItem != null)
                {
                    _didClickOnInnerItems = true;
                    FocusedClass.FocusedItem = clickedOnItem;
                }
                else
                {
                    FocusedClass.FocusedItem = null;                    
                }
                Refresh();
            }

            _isMouseDown = true;
            _mouseX = e.X;
            _mouseY = e.Y;
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isMouseDown = false;
            base.OnMouseUp(e);
        }

        public BaseTemplate GetClassItem(Class parent, float y, float x)
        {
            if (parent.FocusedItem != null && (parent.FocusedItem is Function || parent.FocusedItem is FunctionBody) && x > parent.X + parent.Width)
            {
                var func = parent.FocusedItem is FunctionBody ? ((FunctionBody)parent.FocusedItem).Owner : (Function)parent.FocusedItem;
                func.CalculateBounds();
                if (new RectangleF(parent.X + parent.Width, parent.Y, func.Width, func.Width).Contains(x, y))
                {
                    return func.Body;
                }
            }

            var StartOfItems = y - (parent.Y + 19);
            if (StartOfItems >= 0)
            {
                var index = (int)(StartOfItems == 0 ? 0 : StartOfItems / 21.0f);

                if (index < parent.Fields.Count)
                {
                    return parent.Fields[index];
                }
                else
                {
                    index -= parent.Fields.Count;
                    if (index < parent.Functions.Count)
                    {
                        return parent.Functions[index];
                    }
                }
            }

            return null;
        }

        public Class GetClassFromLocation(float x, float y)
        {
            for (int i = 0; i < Current.Classes.Count; i++)
            {
                var cls = Current.Classes[i];
                if (cls != null)
                {
                    if (new RectangleF(cls.X, cls.Y, cls.Width, cls.Height).Contains(x, y))
                    {
                        return cls;
                    }else if (cls.FocusedItem != null && cls.FocusedItem is Function)
                    {
                        var func = (Function)cls.FocusedItem;
                        func.CalculateBounds();
                        if(new RectangleF(cls.X + cls.Width, cls.Y, func.Width, func.Width).Contains(x, y))
                        {
                            return cls;
                        }
                    }
                    else if (cls.FocusedItem != null && cls.FocusedItem is FunctionBody)
                    {
                        var func = ((FunctionBody)cls.FocusedItem).Owner;
                        func.CalculateBounds();
                        if (new RectangleF(cls.X + cls.Width, cls.Y, func.Width, func.Width).Contains(x, y))
                        {
                            return cls;
                        }
                    }
                }
            }

            return null;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            Current.Y += e.Delta;

            if(!_isMouseDown)
            {
                if (!CalculateHover(e))
                    Refresh();
            }
            else
            {
                Refresh();
            }            

            base.OnMouseWheel(e);
        }

        private bool CalculateHover(MouseEventArgs e)
        {
            if (Current != null && Current.Classes != null)
            {
                BaseTemplate shouldIsHovered = null;

                float mouseX = e.X;
                float mouseY = e.Y;

                var bounds = GetFloatButtonLocation();

                if (bounds.Contains(mouseX, mouseY) && bounds.CircleContains(mouseX, mouseY))
                {
                    shouldIsHovered = FloatingMenu;
                }
                else
                {
                    if(!IsMenuVisible)
                    {
                        mouseX -= Current.X;
                        mouseY -= Current.Y;

                        var cls = GetClassFromLocation(mouseX, mouseY);
                        shouldIsHovered = cls;

                        if (cls != null)
                        {
                            var item = GetClassItem(cls, mouseY, mouseX);
                            if (item != null)
                                shouldIsHovered = item;
                        }
                    }
                    else
                    {
                        shouldIsHovered = GetButtonFromLocation(mouseX, mouseY);
                    }
                }
                
                if (_lastHovered != shouldIsHovered)
                {
                    if (_lastHovered != null)
                        _lastHovered.IsHovered = false;

                    _lastHovered = shouldIsHovered;

                    if (_lastHovered != null)
                        _lastHovered.IsHovered = true;

                    Refresh();
                    return true;
                }                
            }

            return false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if(_isMouseDown && !_didClickOnInnerItems && !IsMenuVisible)
            {
                if(FocusedClass != null)
                {
                    FocusedClass.X -= _mouseX - e.X;
                    FocusedClass.Y -= _mouseY - e.Y;
                }
                else
                {
                    Current.X -= _mouseX - e.X;
                    Current.Y -= _mouseY - e.Y;
                }
                

                _mouseX = e.X;
                _mouseY = e.Y;

                Refresh();
            }
            else
            {
                CalculateHover(e);
            }

            base.OnMouseMove(e);
        }

        public RectangleF GetFloatButtonLocation()
        {
            return new RectangleF(Width - 88, Height - 88, 64, 64);
        }

        public FloatingButton GetButtonFromLocation(float x1, float y1)
        {
            var buttonBounds = GetFloatButtonLocation();



            float y = buttonBounds.Y;

            if (Vertical != null && y1 < buttonBounds.Y)
            {
                var xx = buttonBounds.Y - y1;

                var index = (int)(xx <= 0 ? 0 : xx / (48 + 12));

                if (index < Vertical.Count)
                {
                    var bounds = new RectangleF(buttonBounds.X + 6, y - ((48 + 12) * (index + 1)), 48, 48);
                    if (bounds.Contains(x1, y1) && bounds.CircleContains(x1, y1))
                    {
                        return Vertical[index];
                    }
                }
            }

            float x = buttonBounds.X;

            if (Horizontal != null && y1 > buttonBounds.Y)
            {
                var xx = buttonBounds.X - x1;

                var index = (int)(xx <= 0 ? 0 : xx / (48 + 12));

                if(index < Horizontal.Count)
                {
                    var bounds = new RectangleF(x - ((48 + 12) * (index + 1)), buttonBounds.Y + 6, 48, 48);
                    if (bounds.Contains(x1, y1) && bounds.CircleContains(x1, y1))
                    {
                        return Horizontal[index];
                    }
                }                
            }

            return null;
        }

        public Color GetHoverColor(Color c)
        {
            if(UseBlack(c))
            {
                return ControlPaint.Dark(c);
            }
            else
            {
                return ControlPaint.Light(c);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // clear using the background color
            e.Graphics.Clear(BackColor);
         //   e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
           // e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
           // e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        
            // we cannot render as we do not have a project
            if (Current == null || Current.Classes == null)
                return;
            e.Graphics.TranslateTransform(Current.X, Current.Y);
            // render all classes in the current project
            foreach (var item in Current.Classes)
            {
                item?.OnPaint(e.Graphics, this);
            }

            e.Graphics.TranslateTransform(-Current.X, -Current.Y);

            //if (_isMenuVisible)
            //{
            //    using (SolidBrush layerColor = new SolidBrush(Color.FromArgb(10, Color.White)))
            //        e.Graphics.FillRectangle(layerColor, 0, 0, Width, Height);
            //}

            var buttonBounds = GetFloatButtonLocation();
            // Main button back color
            var mbc = FloatingMenu.GetBackColor(_primaryColor);
            
            e.Graphics.TranslateTransform(3, 2);        
            using (SolidBrush color = new SolidBrush(Color.FromArgb(50, Color.Black)))
                e.Graphics.FillEllipse(color, buttonBounds);

            e.Graphics.TranslateTransform(-3, -2);

            using (SolidBrush color = new SolidBrush(FloatingMenu.IsHovered ? GetHoverColor(mbc) : mbc)) //  Color.FromArgb((byte)(mbc.R + 35), (byte)(mbc.G + 35), (byte)(mbc.B + 35))
                e.Graphics.FillEllipse(color, buttonBounds);

            if(_isMenuVisible)
            {
                e.Graphics.DrawImage(minus_Icon, buttonBounds.X + 16, buttonBounds.Y + 16);
            }
            else
            {
                e.Graphics.DrawImage(add_Icon, buttonBounds.X + 16, buttonBounds.Y + 16);
            }


            if (_isMenuVisible)
            {
                float y = buttonBounds.Y;

                if (Vertical != null)
                {
                    foreach (var item in Vertical)
                    {
                        if (item == null)
                            continue;
                        y -= 48;
                        y -= 12;
                        mbc = item.GetBackColor(_primaryColor);
                        var bounds = new RectangleF(buttonBounds.X + 6, y, 48, 48);

                        e.Graphics.TranslateTransform(3, 2);
                        using (SolidBrush color = new SolidBrush(Color.FromArgb(50, Color.Black)))
                            e.Graphics.FillEllipse(color, bounds);

                        e.Graphics.TranslateTransform(-3, -2);

                        using (SolidBrush color = new SolidBrush(item.IsHovered ? GetHoverColor(mbc) : mbc))
                            e.Graphics.FillEllipse(color, bounds);      
                        
                        if(item.Icon != null)
                        {
                            e.Graphics.DrawImage(item.Icon, bounds.X + 8, bounds.Y + 8);
                        }
                    }
                }

                float x = buttonBounds.X;

                if (Horizontal != null)
                {
                    foreach (var item in Horizontal)
                    {
                        if (item == null)
                            continue;
                        x -= 48;
                        x -= 12;
                        mbc = item.GetBackColor(_primaryColor);
                        var bounds = new RectangleF(x, buttonBounds.Y + 6, 48, 48);

                        e.Graphics.TranslateTransform(3, 2);
                        using (SolidBrush color = new SolidBrush(Color.FromArgb(50, Color.Black)))
                            e.Graphics.FillEllipse(color, bounds);

                        e.Graphics.TranslateTransform(-3, -2);

                        using (SolidBrush color = new SolidBrush(item.IsHovered ? GetHoverColor(mbc) : mbc))
                            e.Graphics.FillEllipse(color, bounds);

                        if (item.Icon != null)
                        {
                            e.Graphics.DrawImage(item.Icon, bounds.X + 8, bounds.Y + 8);
                        }
                    }
                }
            }            
        }
    }
}
