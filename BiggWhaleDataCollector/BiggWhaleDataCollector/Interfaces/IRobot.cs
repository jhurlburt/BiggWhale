using System;

namespace BiggWhaleDataCollector.Interfaces
{
	public interface IRobot
	{
		#region Instance Methods

		bool IsAllowed(string userAgent, Uri uri);

		#endregion
	}
}