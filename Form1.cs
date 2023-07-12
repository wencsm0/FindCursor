using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindCursor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            ExtractFile();
            InitializeComponent();
            this.Visible = false;//可视化属性
            this.FormBorderStyle = FormBorderStyle.None;
            this.TransparencyKey = this.BackColor;
            this.Opacity = 0;

            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = new Icon(Icon_Path);
            this.notifyIcon1.Text = GlobalConstant.AppName;
            this.notifyIcon1.Visible = true;

            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[] { this.aboutToolStripMenuItem, this.exitToolStripMenuItem });
            this.contextMenuStrip1.Name = GlobalConstant.AppName;
            this.contextMenuStrip1.Size = new Size(101, 48);
            contextMenuStrip1.Show();

            Set_MouseHook();
            mouseHook.Start();

            Set_Timer();
            timer.Start();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_EX_APPWINDOW = 0x00040000;
                const int WS_EX_TOOLWINDOW = 0x00000080;

                CreateParams result = base.CreateParams;
                result.ExStyle = result.ExStyle & (~WS_EX_APPWINDOW);
                result.ExStyle = result.ExStyle | WS_EX_TOOLWINDOW;
                result.ExStyle = result.ExStyle | 0x02000000;
                return result;
            }
        }
    }
}
