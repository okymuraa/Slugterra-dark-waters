using System;

namespace AppsMinistry.Domain.Configuration
{
	public sealed class BinarySerializeFormatter : SerializeFormatterAttribute
	{
		public override void Save(object ob, string path)
		{
			throw new NotImplementedException();
		}

		public override object LoadFile(Type type, string path)
		{
			return null;
		}

		public override object LoadTextAsset(Type type, string path)
		{
			return null;
		}
	}
}
