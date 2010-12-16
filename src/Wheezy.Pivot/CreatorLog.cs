
namespace Wheezy.Pivot
{
	public class CreatorLog : Log
	{
		public CreatorLogType LogType { get; private set; }
		public object[] Args { get; private set; }

		public CreatorLog(CreatorLogType logType, params object[] args)
			: base()
		{
			LogType = logType;
			Args = args;
		}		
	}

	public enum CreatorLogType
	{

	}
}
