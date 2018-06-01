using System;
using System.Threading;
using System.Windows.Forms;


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
