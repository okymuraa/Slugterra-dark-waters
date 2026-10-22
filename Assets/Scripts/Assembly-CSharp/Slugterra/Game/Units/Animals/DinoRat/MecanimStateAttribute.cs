using System;

namespace Slugterra.Game.Units.Animals.DinoRat
{
	public class MecanimStateAttribute : Attribute
	{
		private string[] _names;

		public string[] Names
		{
			get
			{
				return _names;
			}
		}

		public string Name
		{
			get
			{
				if (_names != null && _names.Length == 1)
				{
					return _names[0];
				}
				return null;
			}
		}

		public MecanimStateAttribute(params string[] names)
		{
			_names = names;
		}
	}
}
