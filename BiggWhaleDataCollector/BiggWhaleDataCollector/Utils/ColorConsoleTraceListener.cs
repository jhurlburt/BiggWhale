﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BiggWhaleDataCollector.Utils
{
#if !PORTABLE
    public class ColorConsoleTraceListener : TextWriterTraceListener
    {
		#region Readonly & Static Fields

		private readonly Dictionary<TraceEventType, ConsoleColor> m_EventColor =
			new Dictionary<TraceEventType, ConsoleColor>();

		#endregion

		#region Constructors

		public ColorConsoleTraceListener()
            : base(Console.Out)
		{
			m_EventColor.Add(TraceEventType.Verbose, ConsoleColor.DarkGray);
			m_EventColor.Add(TraceEventType.Information, ConsoleColor.Gray);
			m_EventColor.Add(TraceEventType.Warning, ConsoleColor.Yellow);
			m_EventColor.Add(TraceEventType.Error, ConsoleColor.DarkRed);
			m_EventColor.Add(TraceEventType.Critical, ConsoleColor.Red);
			m_EventColor.Add(TraceEventType.Start, ConsoleColor.DarkCyan);
			m_EventColor.Add(TraceEventType.Stop, ConsoleColor.DarkCyan);
		}

		#endregion

		#region Instance Methods

		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
			string message)
		{
			TraceEvent(eventCache, source, eventType, id, "{0}", message);
		}

		public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id,
			string format, params object[] args)
		{
			ConsoleColor originalColor = Console.ForegroundColor;
			Console.ForegroundColor = GetEventColor(eventType, originalColor);
			base.TraceEvent(eventCache, DateTime.UtcNow.ToString(), eventType, id, format, args);
			Console.ForegroundColor = originalColor;
		}

		private ConsoleColor GetEventColor(TraceEventType eventType, ConsoleColor defaultColor)
		{
			return !m_EventColor.ContainsKey(eventType) ? defaultColor : m_EventColor[eventType];
		}

		#endregion
	}
#endif
}