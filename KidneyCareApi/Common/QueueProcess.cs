using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KidneyCareApi.Common
{
    public class QueueProcess
    {
        /// <summary>
        /// 新增日志队列,增加在高并发下的日志处理 异常日志队列，全局唯一
        /// </summary>
        public static Queue<LogInfo> LogInfoQueue = new Queue<LogInfo>();


        public static void LogInfoQueueProcess()
        {
            //开启一个线程扫描日志队列
            ThreadPool.QueueUserWorkItem(a =>
            {
                while (true)
                {
                    if (LogInfoQueue.Any())
                    {
                        var log= LogInfoQueue.Dequeue();
                        if (log != null)
                        {
                            Util.AddLog(log);
                        }
                        else
                        {
                            Thread.Sleep(3000);
                        }
                    }
                    else
                    {
                        Thread.Sleep(3000);
                    }
                }
                // ReSharper disable once FunctionNeverReturns
            });
        }
    }
}
