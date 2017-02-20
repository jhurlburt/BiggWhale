using System;
using System.IO;

using BiggWhaleDataCollector.Extensions;
using BiggWhaleDataCollector.Utils;

namespace BiggWhaleDataCollector.Demo.Extensions
{
	public static class TextWriterExtensions
	{
		#region Class Methods

		public static void WriteLine(this TextWriter writer, ConsoleColor color, string format, params object[] args)
		{
			AspectF.Define.
				NotNull(writer, "writer").
				NotNull(format, "format");

			Console.ForegroundColor = color;
			writer.WriteLine(format, args);
			Console.ResetColor();
		}

		#endregion
	}
}