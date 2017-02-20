using System;
using System.Threading.Tasks;
using BiggWhaleDataCollector.Events;
using BiggWhaleDataCollector.Services;

namespace BiggWhaleDataCollector.Interfaces
{
	public interface IWebDownloader
	{
		#region Instance Properties

		TimeSpan? ConnectionTimeout { get; set; }
		uint? DownloadBufferSize { get; set; }
		uint? MaximumContentSize { get; set; }
		uint? MaximumDownloadSizeInRam { get; set; }
		TimeSpan? ReadTimeout { get; set; }
		int? RetryCount { get; set; }
		TimeSpan? RetryWaitDuration { get; set; }
		bool UseCookies { get; set; }
		string UserAgent { get; set; }

		#endregion

		#region Instance Methods

		PropertyBag Download(CrawlStep crawlStep, CrawlStep referrer, DownloadMethod method);

		void DownloadAsync<T>(CrawlStep crawlStep, CrawlStep referrer, DownloadMethod method,
			Action<RequestState<T>> completed, Action<DownloadProgressEventArgs> progress, T state);

        Task<AsyncRequestState<T>> DownloadAsync<T>(CrawlStep crawlStep, CrawlStep referrer, DownloadMethod method,
            Action<AsyncRequestState<T>> completed, Action<DownloadProgressEventArgs> progress, T state);

        #endregion
    }
}