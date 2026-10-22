using System;

namespace ColorTvPlugin
{
	public class ColorTvCallbacks
	{
		public static Action<string> AdLoaded;

		public static Action<string> AdClosed;

		public static Action<string> AdExpired;

		public static Action<ColorTvError> AdError;

		public static Action<ColorTvCurrency> CurrencyEarned;
	}
}
