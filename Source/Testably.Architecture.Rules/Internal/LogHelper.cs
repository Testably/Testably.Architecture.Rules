using System;

namespace Testably.Architecture.Rules.Internal;

internal static class LogHelper
{
	/// <summary>
	///     Logs the <paramref name="message" /> using the <paramref name="logAction" />
	/// </summary>
	public static void Log(this Action<string>? logAction, string message)
	{
		logAction?.Invoke(
			$"{DateTime.Now:HH:mm:ss.fff}: {message.Replace(Environment.NewLine, $"{Environment.NewLine}              ")}");
	}
}
