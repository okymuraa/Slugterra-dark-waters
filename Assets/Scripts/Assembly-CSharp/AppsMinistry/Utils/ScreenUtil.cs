using UnityEngine;

namespace AppsMinistry.Utils
{
	public static class ScreenUtil
	{
		public static Rect NormalizeRect(Rect rect)
		{
			rect = new Rect(rect.x * (float)Screen.width, (float)Screen.height * rect.y, (float)Screen.width * rect.width, (float)Screen.height * rect.height);
			return rect;
		}

		public static Rect RectCenterX(Rect rect)
		{
			rect = new Rect((float)(Screen.width / 2) - rect.width / 2f * (float)Screen.width, (float)Screen.height * rect.y, (float)Screen.width * rect.width, (float)Screen.height * rect.height);
			return rect;
		}

		public static AspectRation GetAspectRation(int width, int height)
		{
			if ((width == 1024 && height == 768) || (width == 2048 && height == 1536))
			{
				return AspectRation.e4x3;
			}
			if (width == 960 && height == 640)
			{
				return AspectRation.e3x2;
			}
			return AspectRation.e16x9;
		}
	}
}
