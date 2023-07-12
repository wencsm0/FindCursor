using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindCursor
{
    public partial class Form1
    {
        private static string GetFullPath(string name)
        {
            return GlobalConstant.Base_Path + "\\resource\\" + name;
        }

        private void ExtractFile()
        {
            if (!Directory.Exists(GlobalConstant.Base_Path + "\\resource\\"))
            {
                new DirectoryInfo(GlobalConstant.Base_Path + "\\resource\\").Create();
            }
            for (int i = 1; i < Assembly.GetExecutingAssembly().GetManifestResourceNames().Length; i++)
            {
                string resource = Assembly.GetExecutingAssembly().GetManifestResourceNames()[i];
                string[] split = resource.Split('.');
                string filename = split[split.Length-2] + "." + split[split.Length - 1];
                string fullpath = GetFullPath(filename);
                if (!File.Exists(fullpath))
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    BufferedStream input = new BufferedStream(assembly.GetManifestResourceStream(resource));
                    FileStream output = new FileStream(fullpath, FileMode.Create);
                    byte[] data = new byte[1024];
                    int lengthEachRead;
                    while ((lengthEachRead = input.Read(data, 0, data.Length)) > 0)
                    {
                        output.Write(data, 0, lengthEachRead);
                    }
                    output.Flush();
                    output.Close();
                }
            }
        }
    }
}
