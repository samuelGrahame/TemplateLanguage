using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TemplateLanguage
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private Class ClassFromDotNetObject<T>()
        {
            var type = typeof(T);

            var cls = new Class();

            cls.Name = type.Name;

            foreach (var item in type.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public))
            {
                cls.Fields.Add(new Field() { Name = item.Name, Type = item.FieldType });
            }

            foreach (var item in type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public))
            {
                var para = item.GetParameters();
                Field[] fields = null;
                if(para != null && para.Length != 0)
                {
                    fields = new Field[para.Length];
                    for (int i = 0; i < para.Length; i++)
                    {
                        fields[i] = new Field() { Name = para[i].Name, Type = para[i].ParameterType };
                    }
                }

                cls.Functions.Add(new Function() { Name = item.Name, ReturnType = item.ReturnType, ReceiveTypes = fields });
            }
            
            return cls;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //var cls = ClassFromDotNetObject<Int32>();
            //cls.X = 15;
            //cls.Y = 15;
            //cls.Width = 250;
            templateControl1.Current = new Project();
            templateControl1.FloatingMenu.BackColor = Color.Purple;
            //templateControl1.Current.Classes.Add(cls);                        

            //var cls2 = ClassFromDotNetObject<Int32>();
            //cls2.X = 15;
            //cls2.Y = 15;
            //cls2.Width = 250;            
            //templateControl1.Current.Classes.Add(cls2);

            templateControl1.Vertical.Add(new FloatingButton() { OnClick = (fc, tm) => {
                var cls = new Class() { Name = "",
                    X = templateControl1.Current.X + 50,
                    Y = templateControl1.Current.Y + 50, Width = 250 };

                templateControl1.Current.Classes.Add(cls);

                templateControl1.FocusedClass = cls;
            } , Icon = templateControl1.cSharp_Icon });

            templateControl1.Vertical.Add(new FloatingButton()
            {
                OnClick = (fc, tm) => {
                    if(fc != null)
                    {
                        var field = new Field() { Name = "", Type = typeof(int) };
                        fc.Fields.Add(field);
                        fc.FocusedItem = field;
                    }                    
                }, BackColor = Color.Green,
                Icon = templateControl1.number_Icon
            });

            templateControl1.Vertical.Add(new FloatingButton()
            {
                OnClick = (fc, tm) => {
                    if (fc != null)
                    {                        
                        var field = new Field() { Name = "", Type = typeof(decimal) };
                        fc.Fields.Add(field);
                        fc.FocusedItem = field;
                    }
                },
                BackColor = Color.Green,
                Icon = templateControl1.currency_Icon
            });

            templateControl1.Vertical.Add(new FloatingButton()
            {
                OnClick = (fc, tm) => {
                    if (fc != null)
                    {                        
                        var field = new Field() { Name = "", Type = typeof(string) };
                        fc.Fields.Add(field);
                        fc.FocusedItem = field;
                    }
                },
                BackColor = Color.Firebrick, Icon = templateControl1.string_Icon
            });

            templateControl1.Vertical.Add(new FloatingButton()
            {
                OnClick = (fc, tm) => {
                    if (fc != null)
                    {                        
                        var field = new Field() { Name = "", Type = typeof(DateTime) };
                        fc.Fields.Add(field);
                        fc.FocusedItem = field;
                    }
                },
                BackColor = Color.Firebrick,
                Icon = templateControl1.date_Icon
            });

            templateControl1.Vertical.Add(new FloatingButton()
            {
                OnClick = (fc, tm) => {
                    if (fc != null)
                    {
                        var func = new Function() { Name = "", ReturnType = typeof(void) };
                        func.Width = 300;
                        fc.Functions.Add(func);
                        fc.FocusedItem = func;
                    }
                },
                BackColor = Color.FromArgb(86, 156, 214),
                Icon = templateControl1.function_Icon
            });


            templateControl1.Horizontal.Add(new FloatingButton()
            {
                OnClick = (fc, tm) => {
                    if (fc != null && templateControl1.Current != null)
                    {
                        if(fc.FocusedItem != null)
                        {
                            if(fc.FocusedItem is Field)
                            {
                                int index = fc.Fields.IndexOf((Field)fc.FocusedItem);
                                fc.Fields.Remove((Field)fc.FocusedItem);
                                if (index == fc.Fields.Count)
                                    index--;

                                if (index < fc.Fields.Count && index >= 0)
                                    fc.FocusedItem = fc.Fields[index];

                            }
                            else if (fc.FocusedItem is Function)
                            {
                                int index = fc.Functions.IndexOf((Function)fc.FocusedItem);
                                fc.Functions.Remove((Function)fc.FocusedItem);
                                if (index == fc.Functions.Count)
                                    index--;

                                if (index < fc.Functions.Count && index >= 0)
                                    fc.FocusedItem = fc.Functions[index];
                            }                                                        
                        }
                        else
                        {
                            templateControl1.Current.Classes.Remove(fc);
                        }                        
                        
                    }
                },
                BackColor = templateControl1.PrimaryColor,
                Icon = templateControl1.delete_Icon
            });

            templateControl1.Horizontal.Add(new FloatingButton()
            {
                OnClick = (fc, tm) => {
                    if (fc != null && templateControl1.Current != null)
                    {
                        templateControl1.Current.Classes.Remove(fc);
                        templateControl1.Current.Classes.Add(fc);
                    }
                },
                BackColor = templateControl1.PrimaryColor,
                Icon = templateControl1.bringtoFront_Icon
            });

            templateControl1.Horizontal.Add(new FloatingButton()
            {
                OnClick = (fc, tm) => {
                    if (fc != null && templateControl1.Current != null)
                    {
                        templateControl1.Current.Classes.Remove(fc);
                        templateControl1.Current.Classes.Insert(0, fc);
                    }
                },
                BackColor = templateControl1.PrimaryColor,
                Icon = templateControl1.sendtoBack_Icon
            });

            templateControl1.Vertical[0].OnClick(null, templateControl1);

            templateControl1.FocusedClass.Name = "Program";
            templateControl1.Vertical[5].OnClick(templateControl1.FocusedClass, templateControl1);
            var funcMain = (Function)templateControl1.FocusedClass.FocusedItem;
            funcMain.Name = "Main";
            templateControl1.FocusedClass.FocusedItem = funcMain.Body;
            //templateControl1.FocusedClass = cls;            
        }
    }
}
