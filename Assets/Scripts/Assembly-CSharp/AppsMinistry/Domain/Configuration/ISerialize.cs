using System.IO;

namespace AppsMinistry.Domain.Configuration
{
	public interface ISerialize
	{
		void Serialize(BinaryWriter writer);

		void Deserialize(BinaryReader reader);
	}
}
