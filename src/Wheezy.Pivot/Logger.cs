using System;

namespace Wheezy.Pivot
{
	public static class Logger
	{
		#region Events

		public static event EventHandler<SimpleEventArgs<Log>> LogReceived;

		#endregion

		#region Methods

		public static void Log(Log log)
		{
			if (log is DebugLog)
			{
				#if !DEBUG
					return;
				#endif
			}

			OnLogReceived(log.Sender, log);
		}

        public static void OnLogReceived(object sender, Log log)
        {
            if (LogReceived != null)
            {
                LogReceived(sender, new SimpleEventArgs<Log>(log));
            }
        }

		#endregion
	}

    public class SimpleEventArgs<T> : EventArgs
    {
        public T Result { get; set; }
        public SimpleEventArgs(T result)
        {
            Result = result;
        }
    }
}
