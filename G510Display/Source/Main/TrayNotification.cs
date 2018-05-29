using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using G510Display.Source;
using G510Display.Source.Workerthread;

namespace G510Display
{
  class TrayNotification : ApplicationContext
  {
    public static TrayNotification CurrentTrayNotification;
    NotifyIcon notifyIcon = new NotifyIcon();
    G510Display.Source.Forms.ConfigurationForm configWindow = new G510Display.Source.Forms.ConfigurationForm();
    ThreadPolling PollingThread = new ThreadPolling();
    
    public TrayNotification()
    {
      CurrentTrayNotification = this;
      MenuItem configMenuItem = new MenuItem("Configuration", new EventHandler(ShowConfig));
      MenuItem exitMenuItem = new MenuItem("Exit", new EventHandler(Exit));

      notifyIcon.Icon = G510Display.Properties.Resources.G510DisplayIcon;
      notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { configMenuItem, exitMenuItem });
      notifyIcon.Visible = true;

      
      PollingThread.StartThread();
    }
    void Exit(object sender, EventArgs e)
    {
      PollingThread.StopAndWait();
      CleanExit();
    }
    public void CleanExit()
    {
      notifyIcon.Visible = false;
      Application.Exit();
    }
    void ShowConfig(object sender, EventArgs e)
    {
      if (configWindow.Visible)
        configWindow.Focus();
      else
        configWindow.ShowDialog();
    }
  }
}
