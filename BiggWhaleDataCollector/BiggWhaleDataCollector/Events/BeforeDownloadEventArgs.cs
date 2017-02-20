using System.ComponentModel;

namespace BiggWhaleDataCollector.Events
{
	public class BeforeDownloadEventArgs : CancelEventArgs
	{
		#region Constructors

		internal BeforeDownloadEventArgs(bool cancel, CrawlStep crawlStep)
			: base(cancel)
		{
			CrawlStep = crawlStep;
		}

		#endregion

		#region Instance Properties

		public CrawlStep CrawlStep { get; private set; }

		#endregion
	}
}