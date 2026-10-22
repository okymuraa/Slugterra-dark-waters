using System;

namespace DG.Tweening
{
	public static class DOTweenAnimationExtensions
	{
		public static bool IsSameOrSubclassOf(this Type t, Type tBase)
		{
			return t.IsSubclassOf(tBase) || t == tBase;
		}
	}
}
