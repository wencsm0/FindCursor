using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FindCursor.InputHelp
{
    internal static class MouseCheck
    {
        // 晃动次数阈值
        readonly static int Shake_Times_Threshold = 3;

        // 晃动幅度最大阈值(px)
        readonly static int Shake_Max_Range_Threshold = 400;

        // 晃动幅度最小阈值(px)
        readonly static int Shake_Min_Range_Threshold = 60;


        public static bool Is_Shakeing(List<Point> posList)
        {
            if (posList.Count < 3)
            {
                return false;
            }

            int[] Y = posList.Select(p => p.X).ToArray();

            List<int> extremePoint = new List<int>();
            for (int i = 0; i < Y.Length - 2; i++)
            {
                int diff1 = Y[i + 1] - Y[i];
                int diff2 = Y[i + 2] - Y[i + 1];

                if (diff1 == 0 || diff2 == 0)
                {
                    continue;
                }

                if (Math.Sign(diff1) == Math.Sign(diff2))
                {
                    continue;
                }

                if (Math.Abs(diff1) < Shake_Min_Range_Threshold || Math.Abs(diff2) < Shake_Min_Range_Threshold)
                {
                    continue;
                }

                extremePoint.Add(Y[i + 1]);
            }


            int shakeCnt = extremePoint.Count;
            if (shakeCnt < Shake_Times_Threshold)
            {
                return false;
            }

            int maxPoint = extremePoint.Max();
            int minPoint = extremePoint.Min();
            if ((maxPoint - minPoint) < Shake_Max_Range_Threshold)
            {
                return false;
            }

            return true;
        }
    }
}
