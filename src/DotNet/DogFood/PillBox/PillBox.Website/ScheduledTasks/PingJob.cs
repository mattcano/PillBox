using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace PillBox.Website.ScheduledTasks
{
    public class PingJob : IJob
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Execute(IJobExecutionContext context)
        {
            //log.Info("Ping. Still Alive.");
            try
            {
                var tcs = new TaskCompletionSource<string>();
                var wc = new WebClient();
                wc.DownloadStringCompleted += (s, e) =>
                {
                    if (e.Error != null) tcs.TrySetException(e.Error);
                    else if (e.Cancelled) tcs.TrySetCanceled();
                    else tcs.TrySetResult(e.Result);
                };
                wc.DownloadStringAsync(new Uri("http://localhost"));
            }
            catch
            {
                //Do nothing
            }
        }
    }
}