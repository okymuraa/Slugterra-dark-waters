using System;

namespace ColorTvPlugin
{
	public class ColorTvError
	{
		public enum ColorTvErrorCode
		{
			UNKNOWN_ERROR = 0,
			NO_ADS = 1,
			NETWORK_ERROR = 2,
			INVALID_REQUEST = 3,
			INTERNAL_SERVER_ERROR = 4,
			INTERNAL_SDK_ERROR = 5
		}

		public string PlacementId { get; private set; }

		public ColorTvErrorCode ErrorCode { get; private set; }

		public string ErrorMessage { get; private set; }

		private ColorTvError(string placementId, ColorTvErrorCode errorCode, string errorMessage)
		{
			PlacementId = placementId;
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
		}

		public static ColorTvError FromString(string result)
		{
			string[] array = result.Split(':');
			if (array.Length < 3)
			{
				return null;
			}
			return new ColorTvError(array[0], (ColorTvErrorCode)Enum.Parse(typeof(ColorTvErrorCode), array[1]), array[2]);
		}
	}
}
