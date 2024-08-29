using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateSelf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            if (args.Length > 0)
            {
                string autoupdatePath = args[0];
                MessageBox.Show(autoupdatePath);
                if (System.IO.File.Exists(autoupdatePath + ".delete"))
                {
                    System.IO.File.Delete(autoupdatePath + ".delete");
                }
                File.Move(autoupdatePath, autoupdatePath + ".delete");
                File.Copy(autoupdatePath, autoupdatePath);

                //if (autoupdate.Key != null)
                //{
                //    autoupdatePath = autoupdate.Value;
                //    string filename = Assembly.GetExecutingAssembly().Location;
                //    if (System.IO.File.Exists(filename + ".delete"))
                //    {
                //        System.IO.File.Delete(filename + ".delete");
                //    }
                //    File.Move(filename, filename + ".delete");
                //    File.Copy(autoupdate.Value, filename);
                //    //ProcessStartInfo p = new ProcessStartInfo();
                //    //p.FileName = "test.bat";
                //    //p.Arguments = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
                //    //p.WindowStyle = ProcessWindowStyle.Hidden;
                //    //Process.Start(p);
                //}
            }

        }
    }
}
