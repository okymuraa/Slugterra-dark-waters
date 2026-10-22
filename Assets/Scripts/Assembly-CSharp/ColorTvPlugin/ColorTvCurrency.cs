using UnityEngine;

namespace ColorTvPlugin
{
	public class ColorTvCurrency
	{
		public string Placement { get; private set; }

		public string Type { get; private set; }

		public int Amount { get; private set; }

		private ColorTvCurrency(string placement, string type, int amount)
		{
			Placement = placement;
			Type = type;
			Amount = amount;
		}

		public static ColorTvCurrency FromString(string result)
		{
			Debug.Log("coin result: '" + result + "'");
			string[] array = result.Split(':');
			if (array.Length < 3)
			{
				return null;
			}
			return new ColorTvCurrency(array[0], array[1], int.Parse(array[2]));
		}
	}
}
