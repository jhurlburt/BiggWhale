using System;

namespace BiggWhaleDataCollector.Interfaces
{
	public interface IFilter
	{
		#region Instance Methods

		bool Match(Uri uri, CrawlStep referrer);

		#endregion
	}
}