using System;

namespace BiggWhaleDataCollector.Interfaces
{
	public interface IPipelineStepWithTimeout : IPipelineStep
	{
		TimeSpan ProcessorTimeout { get; }
	}
}