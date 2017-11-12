namespace TemplateLanguage
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.templateControl1 = new TemplateLanguage.TemplateControl();
            this.SuspendLayout();
            // 
            // templateControl1
            // 
            this.templateControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.templateControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templateControl1.FocusedClass = null;
            this.templateControl1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.templateControl1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(230)))), ((int)(((byte)(245)))));
            this.templateControl1.IsMenuVisible = false;
            this.templateControl1.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(70)))));
            this.templateControl1.ItemFocusedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(53)))));
            this.templateControl1.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(92)))));
            this.templateControl1.ItemTypeColor = System.Drawing.Color.FromArgb(((int)(((byte)(78)))), ((int)(((byte)(201)))), ((int)(((byte)(176)))));
            this.templateControl1.Location = new System.Drawing.Point(0, 0);
            this.templateControl1.Name = "templateControl1";
            this.templateControl1.NumberColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(206)))), ((int)(((byte)(168)))));
            this.templateControl1.PrimaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.templateControl1.PrimaryHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(151)))), ((int)(((byte)(234)))));
            this.templateControl1.SecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.templateControl1.Size = new System.Drawing.Size(784, 562);
            this.templateControl1.StringColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(157)))), ((int)(((byte)(133)))));
            this.templateControl1.TabIndex = 0;
            this.templateControl1.Text = "templateControl1";
            this.templateControl1.TextEditorSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(79)))), ((int)(((byte)(120)))));
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.templateControl1);
            this.Name = "frmMain";
            this.Text = "frmMain";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private TemplateControl templateControl1;
    }
}