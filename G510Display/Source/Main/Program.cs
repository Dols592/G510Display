using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using G510Display.Source.Logitech;
using G510Display.Source.DrawImage;

namespace G510Display
{
  static class Program
  {
    static Mutex mutex = new Mutex(true, "{D507F92D-F52F-4CF8-A24B-72272DF825EF}");
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      if (!mutex.WaitOne(TimeSpan.Zero, true))
        return;

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      //Application.Run(new Form1());
      Application.Run(new TrayNotification());
    }
  }
}
