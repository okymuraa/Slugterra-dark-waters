using System;

namespace AppsMinistry.Domain.Configuration
{
	public class ConfigPathAttribute : Attribute
	{
		private string _path;

		public virtual string Path
		{
			get
			{
				return _path.Replace(".xml", string.Empty);
			}
		}

		public ConfigPathAttribute(string path)
		{
			_path = path;
		}
	}
}
