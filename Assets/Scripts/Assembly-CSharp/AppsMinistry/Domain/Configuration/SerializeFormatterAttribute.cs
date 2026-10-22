using System;

namespace AppsMinistry.Domain.Configuration
{
	public abstract class SerializeFormatterAttribute : Attribute
	{
		public abstract void Save(object ob, string path);

		public abstract object LoadFile(Type type, string path);

		public abstract object LoadTextAsset(Type type, string path);
	}
}
