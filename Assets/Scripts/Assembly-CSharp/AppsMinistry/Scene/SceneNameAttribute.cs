using System;

namespace AppsMinistry.Scene
{
	public class SceneNameAttribute : Attribute
	{
		private string _name;

		private bool _isHD;

		private bool _isMultiplayer;

		public string Name
		{
			get
			{
				return _name;
			}
		}

		public bool IsHD
		{
			get
			{
				return _isHD;
			}
		}

		public bool IsMultiplayer
		{
			get
			{
				return _isMultiplayer;
			}
		}

		public SceneNameAttribute(string name, string hd_name = "", bool isHD = false, bool isMultiplayer = false)
		{
			_name = ((!isHD) ? name : hd_name);
			_isHD = isHD;
			_isMultiplayer = isMultiplayer;
		}
	}
}
