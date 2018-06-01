using System.Threading;

namespace G510Display.Source.Workerthread
{
  class ThreadPolling
  {
    private bool stopping;
    private ManualResetEvent stoppedEvent;
    G510Display_Main Main = new G510Display_Main();
    public void StartThread()
    {
      stopping = false;
      stoppedEvent = new ManualResetEvent(false);
      ThreadPool.QueueUserWorkItem(new WaitCallback(ServiceWorkerThread));
    }

    public void StopAndWait()
    {
      stopping = true;
      stoppedEvent.WaitOne();
    }
    private void ServiceWorkerThread(object state)
    {
      Main.Init();

      while (!stopping)
      {
        Main.DoMainCycle();
        Thread.Sleep(10);
      }

      this.stoppedEvent.Set();
      if (!stopping)
      {
        TrayNotification.CurrentTrayNotification.CleanExit();
      }
    }
  }
}
