using FindCursor.InputHelp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using static FindCursor.InputHelp.MouseHook;

namespace FindCursor
{
    public partial class Form1
    {
        bool seted = false;

        Point pos = new Point();

        List<Point> posList = new List<Point>();

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Set_On_Exit();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                Set_On_Exit();
                this.Dispose();
                this.Close();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            posList.Add(pos);
            Point lastPoint = posList[posList.Count - 1];

            int stopPos = Stop_Interval_Threshold / Sampling_Interval;
            if (posList.Count > stopPos)
            {
                Point stopPoint = posList[posList.Count - stopPos];

                if (lastPoint.Equals(stopPoint))
                {
                    posList.Clear();
                }

                if (MouseCheck.Is_Shakeing(posList))
                {
                    Set_Cursor();
                }
                else
                {
                    Reset_Cursor();
                }
            }

        }

        private void Set_Timer()
        {
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Interval = Sampling_Interval;
            timer.SynchronizingObject = this;
        }

        private void Set_MouseHook()
        {
            MyMouseEventHandler handler = (s, e) =>
            {
                pos = e.p;
            };
            mouseHook.AddMouseHandler(handler);
        }

        private void Set_Cursor()
        {
            if (!seted)
            {
                CursorControl.SetCursor(Cursor_Path, Cursor_Width, Cursor_Height);
                seted = true;
            }
        }

        private void Reset_Cursor()
        {
            if (seted)
            {
                CursorControl.ResetCursor();
                seted = false;
            }
        }

        private void Set_On_Exit()
        {
            CursorControl.ResetCursor();
        }
    }
}
