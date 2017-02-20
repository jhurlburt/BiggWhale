using System;

using BiggWhaleDataCollector.Interfaces;

namespace BiggWhaleDataCollector.Services
{
	public class DummyRobot : IRobot
	{
		#region IRobot Members

		public bool IsAllowed(string userAgent, Uri uri)
		{
			return true;
		}

		#endregion
	}
}